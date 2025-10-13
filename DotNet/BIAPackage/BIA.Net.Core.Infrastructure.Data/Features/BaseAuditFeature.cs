// <copyright file="BaseAuditFeature.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Principal;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Audit.Core;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using static System.Formats.Asn1.AsnWriter;

    /// <summary>
    /// The Audit Feature.
    /// </summary>
    public class BaseAuditFeature : IAuditFeature
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IReadOnlyCollection<IAuditMapper> auditMappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAuditFeature" /> class.
        /// </summary>
        /// <param name="commonFeaturesConfigurationOptions">The common features configuration options.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        public BaseAuditFeature(IOptions<CommonFeatures> commonFeaturesConfigurationOptions, IServiceProvider serviceProvider)
        {
            Audit.Core.Configuration.AuditDisabled = true;
            AuditConfiguration auditConfiguration = commonFeaturesConfigurationOptions.Value.AuditConfiguration;
            this.IsActive = auditConfiguration?.IsActive == true;
            this.auditMappers = serviceProvider.GetServices<IAuditMapper>().ToList();

            // Audit
            if (this.IsActive)
            {
#pragma warning disable CA2214 // Do not call overridable methods in constructors
#pragma warning disable S1699 // Constructors should only call non-overridable methods
                this.UseAuditFeatures(serviceProvider);
#pragma warning restore S1699 // Constructors should only call non-overridable methods
#pragma warning restore CA2214 // Do not call overridable methods in constructors
                Audit.Core.Configuration.AuditDisabled = false;

                // Log some Audit in dedicated table and all other in AuditLog
                Audit.Core.Configuration.Setup()
                    .UseEntityFramework(_ => _
                        .AuditTypeMapper(type => this.AuditTypeMapper(type))
                        .AuditEntityAction<IAudit>(async (evt, entry, audit) =>
                        {
                            if (evt.Environment.Exception != null)
                            {
                                return false;
                            }

                            return audit switch
                            {
                                AuditLog auditLog => await this.GeneralAudit(evt, entry, auditLog),
                                IAuditEntity auditEntity => await this.DedicatedAudit(evt, entry, auditEntity),
                                _ => throw new BadBiaFrameworkUsageException($"Unknown audit of type {audit.GetType()}"),
                            };
                        })
                        .IgnoreMatchedProperties(t => t.Name == "AuditLog")); // do not copy properties for generic audit
            }

            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        protected bool IsActive { get; }

        /// <summary>
        /// Configure the Audit feature in order to retrieve
        /// the current (associated to the request) user.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider.</param>
        public virtual void UseAuditFeatures(IServiceProvider serviceProvider)
        {
            if (this.IsActive)
            {
                Audit.Core.Configuration.AddOnSavingAction(scope =>
                {
                    BiaClaimsPrincipal principal = serviceProvider.GetRequiredService<IPrincipal>() as BiaClaimsPrincipal;
                    scope.Event.Environment.CustomFields[Common.BiaConstants.Audit.UserLoginCustomField] = principal.Identity.Name;
                });
            }
        }

        /// <summary>
        /// Audits the type mapper.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type of the Audit entity.</returns>
        public virtual Type AuditTypeMapper(Type type)
        {
            return type.Name switch
            {
                _ => typeof(AuditLog),
            };
        }

        /// <summary>
        /// Save general audit.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="auditEntity">The audit entity.</param>
        /// <returns>True if a change is logged in audit table.</returns>
        protected virtual Task<bool> GeneralAudit(AuditEvent evt, EventEntry entry, AuditLog auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != Common.BiaConstants.Audit.UpdateAction)
            {
                auditEntity.Table = entry.Table;
                auditEntity.PrimaryKey = JsonSerializer.Serialize(entry.PrimaryKey);
                auditEntity.AuditDate = DateTime.UtcNow;
                auditEntity.AuditUserLogin = evt.Environment.CustomFields[Common.BiaConstants.Audit.UserLoginCustomField].ToString();
                auditEntity.AuditAction = entry.Action;
                switch (entry.Action)
                {
                    case Common.BiaConstants.Audit.UpdateAction:
                        auditEntity.AuditChanges = JsonSerializer.Serialize(entry.Changes);
                        break;
                    case Common.BiaConstants.Audit.InsertAction:
                        auditEntity.AuditChanges = JsonSerializer.Serialize(entry.ColumnValues);
                        break;
                    case Common.BiaConstants.Audit.DeleteAction:
                        auditEntity.AuditChanges = JsonSerializer.Serialize(entry.ColumnValues);
                        break;
                }

                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Save dedicated audit.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="auditEntity">The audit entity.</param>
        /// <returns>True if a change is logged in audit table.</returns>
        protected virtual async Task<bool> DedicatedAudit(AuditEvent evt, EventEntry entry, IAuditEntity auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != Common.BiaConstants.Audit.UpdateAction)
            {
                // Retrieve the audited entity's id
                auditEntity.EntityId = auditEntity.Id.ToString();

                // Reset to zero the audit ID to avoid insertion error
                auditEntity.Id = 0;

                // Load all unloaded direct references of the entity before filling specific properties
                var entityEntry = entry.GetEntry();
                foreach (var reference in entityEntry.References.Where(r => !r.IsLoaded))
                {
                    await reference.LoadAsync();
                }

                // Fill specific properties based on the audited entity
                auditEntity.FillSpecificProperties(entityEntry.Entity);

                auditEntity.AuditDate = DateTime.UtcNow;
                auditEntity.AuditUserLogin = evt.Environment.CustomFields[Common.BiaConstants.Audit.UserLoginCustomField].ToString();
                auditEntity.AuditAction = entry.Action;

                var auditMapper = this.auditMappers.FirstOrDefault(mapper => mapper.EntityType == entityEntry.Entity.GetType());
                switch (entry.Action)
                {
                    case Common.BiaConstants.Audit.UpdateAction:
                        await this.SetUpdateAuditChanges(auditEntity, entityEntry, entry.Changes, auditMapper);
                        break;
                    case Common.BiaConstants.Audit.InsertAction:
                        await SetInsertOrDeleteAuditChanges(auditEntity, entityEntry, auditMapper, true);
                        break;
                    case Common.BiaConstants.Audit.DeleteAction:
                        await SetInsertOrDeleteAuditChanges(auditEntity, entityEntry, auditMapper, false);
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the changes data of an insert or delete audit.
        /// </summary>
        /// <param name="auditEntity">Audit entity.</param>
        /// <param name="entityEntry">Audited entity entry.</param>
        /// <param name="auditMapper">The audit mapper.</param>
        /// <param name="isInsertAudit">Indicates if it concerns insert audit (otherwise, delete).</param>
        /// <returns><see cref="Task"/>.</returns>
        /// <exception cref="BadBiaFrameworkUsageException">.</exception>
        private static async Task SetInsertOrDeleteAuditChanges(IAuditEntity auditEntity, EntityEntry entityEntry, IAuditMapper auditMapper, bool isInsertAudit)
        {
            var auditChanges = new List<AuditChange>();
            var auditedEntityProperties = entityEntry.Entity
                .GetType()
                .GetProperties()
                .Where(p => !entityEntry.References.Any(r => r.Metadata.Name == p.Name) && p.GetCustomAttribute<AuditIgnoreAttribute>() is null)
                .ToList();

            foreach (var auditedEntityProperty in auditedEntityProperties)
            {
                var auditedEntityPropertyValue = auditedEntityProperty.GetValue(entityEntry.Entity);
                var auditPropertyMapper = auditMapper?.AuditPropertyMappers.FirstOrDefault(x => x.EntityPropertyIdentifierName == auditedEntityProperty.Name);
                if (auditPropertyMapper is null)
                {
                    auditChanges.Add(new AuditChange(
                        auditedEntityProperty.Name,
                        isInsertAudit ? null : auditedEntityPropertyValue,
                        isInsertAudit ? null : auditedEntityPropertyValue?.ToString(),
                        isInsertAudit ? auditedEntityPropertyValue : null,
                        isInsertAudit ? auditedEntityPropertyValue?.ToString() : null));
                    continue;
                }

                var linkedEntityPropertyReference = entityEntry.References.FirstOrDefault(x => x.Metadata.ClrType == auditPropertyMapper.LinkedEntityType)
                    ?? throw new BadBiaFrameworkUsageException($"Unable to find any reference of type {auditPropertyMapper.LinkedEntityType} into {entityEntry.Entity.GetType()}");

                if (!linkedEntityPropertyReference.IsLoaded)
                {
                    await linkedEntityPropertyReference.LoadAsync();
                }

                var linkedEntity = linkedEntityPropertyReference.TargetEntry.Entity;
                var linkedEntityPropertyDisplayPropertyInfo = linkedEntity.GetType().GetProperty(auditPropertyMapper.LinkedEntityPropertyDisplayName)
                    ?? throw new BadBiaFrameworkUsageException($"Unable to find property {auditPropertyMapper.LinkedEntityPropertyDisplayName} into {linkedEntity.GetType()}");

                var valueDisplay = linkedEntityPropertyDisplayPropertyInfo.GetValue(linkedEntity, null)?.ToString();
                auditChanges.Add(new AuditChange(
                    auditedEntityProperty.Name,
                    isInsertAudit ? null : auditedEntityPropertyValue,
                    isInsertAudit ? null : valueDisplay,
                    isInsertAudit ? auditedEntityPropertyValue : null,
                    isInsertAudit ? valueDisplay : null));
            }

            auditEntity.AuditChanges = JsonSerializer.Serialize(auditChanges);
        }

        /// <summary>
        /// Set the changes data of an update audit.
        /// </summary>
        /// <param name="auditEntity">Audit entity.</param>
        /// <param name="entityEntry">Audited entity entry.</param>
        /// <param name="changes">Audited entity changes.</param>
        /// <param name="auditMapper">The audit mapper.</param>
        /// <returns><see cref="Task"/>.</returns>
        /// <exception cref="BadBiaFrameworkUsageException">.</exception>
        private async Task SetUpdateAuditChanges(IAuditEntity auditEntity, EntityEntry entityEntry, IReadOnlyList<EventEntryChange> changes, IAuditMapper auditMapper)
        {
            var auditChanges = new List<AuditChange>();
            var realChanges = changes.Where(c => c.NewValue?.ToString() != c.OriginalValue?.ToString()).ToList();

            foreach (var change in realChanges)
            {
                var auditPropertyMapper = auditMapper?.AuditPropertyMappers.FirstOrDefault(x => x.EntityPropertyIdentifierName == change.ColumnName);
                if (auditPropertyMapper is null)
                {
                    auditChanges.Add(new AuditChange(
                        change.ColumnName,
                        change.OriginalValue,
                        change.OriginalValue?.ToString(),
                        change.NewValue,
                        change.NewValue?.ToString()));
                    continue;
                }

                var linkedEntityPropertyReference = entityEntry.References.FirstOrDefault(x => x.Metadata.ClrType == auditPropertyMapper.LinkedEntityType)
                    ?? throw new BadBiaFrameworkUsageException($"Unable to find any reference of type {auditPropertyMapper.LinkedEntityType} into {entityEntry.Entity.GetType()}");

                if (!linkedEntityPropertyReference.IsLoaded)
                {
                    await linkedEntityPropertyReference.LoadAsync();
                }

                var linkedEntity = linkedEntityPropertyReference.TargetEntry.Entity;
                var linkedEntityPropertyDisplayPropertyInfo = linkedEntity.GetType().GetProperty(auditPropertyMapper.LinkedEntityPropertyDisplayName)
                    ?? throw new BadBiaFrameworkUsageException($"Unable to find property {auditPropertyMapper.LinkedEntityPropertyDisplayName} into {linkedEntity.GetType()}");

                var newDisplay = linkedEntityPropertyDisplayPropertyInfo.GetValue(linkedEntity, null)?.ToString();
                var originalDisplay = await this.GetEntityDisplayValue(linkedEntity.GetType(), change.OriginalValue, auditPropertyMapper.LinkedEntityPropertyDisplayName)
                    ?? await this.RetrieveOriginalDisplayChangeFromPreviousAudit(auditEntity, change.ColumnName);

                auditChanges.Add(new AuditChange(
                    change.ColumnName,
                    change.OriginalValue,
                    originalDisplay,
                    change.NewValue,
                    newDisplay));
            }

            auditEntity.AuditChanges = JsonSerializer.Serialize(auditChanges);
        }

        /// <summary>
        /// Retrieve the value of the <paramref name="displayPropertyName"/> of an <paramref name="entityType"/> by its <paramref name="identifierValue"/>.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="identifierValue">The entity identifier value.</param>
        /// <param name="displayPropertyName">The entity display property name.</param>
        /// <returns>The display property name value of the entity as <see cref="string"/>.</returns>
        /// <exception cref="BadBiaFrameworkUsageException">.</exception>
        private async Task<string> GetEntityDisplayValue(Type entityType, object identifierValue, string displayPropertyName)
        {
            if (identifierValue == null)
            {
                return null;
            }

            using var scope = this.serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IQueryableUnitOfWorkNoTracking>();
            var entityTypeInfo = unitOfWork.FindEntityType(entityType);
            var primaryKey = entityTypeInfo.FindPrimaryKey();
            if (primaryKey.Properties.Count != 1)
            {
                throw new BadBiaFrameworkUsageException("Composite PK not supported here.");
            }

            // Retrieve entity PK identifier property data
            var identifierProperty = primaryKey.Properties[0];
            var identifierPropertyName = identifierProperty.Name;
            var identifierPropertyType = identifierProperty.ClrType;

            // Prepare expression query
            var entitySetQuery = unitOfWork.RetrieveSet(entityType);
            var entityExpressionParameter = Expression.Parameter(entityType);

            // Set expression : "WHERE entity.identifierProperty = identifierKey"
            var identifierKey = Common.Helpers.ObjectHelper.ConvertKey(identifierValue, identifierPropertyType);
            var identifierKeyPropertyExpression = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                [identifierPropertyType],
                entityExpressionParameter,
                Expression.Constant(identifierPropertyName));
            var identifierKeyPredicateExpression = Expression.Lambda(
                Expression.Equal(
                    identifierKeyPropertyExpression,
                    Expression.Constant(identifierKey, identifierPropertyType)),
                entityExpressionParameter);
            var identifierKeyWhereExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                [entityType],
                entitySetQuery.Expression,
                identifierKeyPredicateExpression);

            // Set expression : "SELECT entity.displayPropertyName"
            var displayPropertyExpression = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                [typeof(object)],
                entityExpressionParameter,
                Expression.Constant(displayPropertyName));
            var displayPropertySelectorExpression = Expression.Lambda(displayPropertyExpression, entityExpressionParameter);

            // Call expression "SELECT entity.displayPropertyName WHERE entity.identifierProperty = identifierKey"
            var displayPropertySelectExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                [entityType, typeof(object)],
                identifierKeyWhereExpression,
                displayPropertySelectorExpression);

            // Create query
            var result = await entitySetQuery.Provider
                .CreateQuery<object>(displayPropertySelectExpression)
                .FirstOrDefaultAsync();

            return result?.ToString();
        }

        /// <summary>
        /// Retrieve the value of <paramref name="changePropertyName"/> from previous audit based on <paramref name="auditEntity"/>.
        /// </summary>
        /// <param name="auditEntity">The audit entity.</param>
        /// <param name="changePropertyName">The change property name.</param>
        /// <returns>The previous value of change as <see cref="string"/>.</returns>
        /// <exception cref="BadBiaFrameworkUsageException">.</exception>
        private async Task<string> RetrieveOriginalDisplayChangeFromPreviousAudit(IAuditEntity auditEntity, string changePropertyName)
        {
            using var scope = this.serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IQueryableUnitOfWorkNoTracking>();

            var previousAudit = await unitOfWork
                .RetrieveSet(auditEntity.GetType())
                .Cast<IAuditEntity>()
                .Where(x => x.EntityId.Equals(auditEntity.EntityId))
                .OrderByDescending(x => x.AuditDate)
                .Select(x => new { x.AuditAction, x.AuditChanges })
                .Take(1)
                .FirstOrDefaultAsync();

            if (previousAudit is null)
            {
                return null;
            }

            try
            {
                var auditChanges = JsonSerializer.Deserialize<List<AuditChange>>(previousAudit.AuditChanges);
                return auditChanges.FirstOrDefault(x => x.ColumnName == changePropertyName)?.NewDisplay;
            }
            catch (JsonException)
            {
                try
                {
                    using var doc = JsonDocument.Parse(previousAudit.AuditChanges);
                    var root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        var previousPropertyValue = root.EnumerateObject().Where(x => x.Name.Equals(changePropertyName)).Select(x => x.Value).FirstOrDefault();
                        return previousPropertyValue.ValueKind switch
                        {
                            JsonValueKind.Null => null,
                            JsonValueKind.String => previousPropertyValue.GetString(),
                            _ => previousPropertyValue.ToString(),
                        };
                    }
                }
                catch (JsonException ex)
                {
                    throw new BadBiaFrameworkUsageException($"Unable to parse previous changes for {auditEntity.GetType()} with ID {auditEntity.Id} : {ex.Message}", ex);
                }
            }

            return null;
        }
    }
}

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
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                                AuditEntity auditEntity => await this.DedicatedAudit(evt, entry, auditEntity),
                                _ => throw new Common.Exceptions.BadBiaFrameworkUsageException($"Unknown audit of type {audit.GetType()}"),
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
                    scope.Event.Environment.CustomFields["UserLogin"] = principal.Identity.Name;
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
                "User" => typeof(UserAudit),
                _ => typeof(AuditLog),
            };
        }

        /// <summary>
        /// Generals the audit.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="auditEntity">The audit entity.</param>
        /// <returns>True if a change is logged in audit table.</returns>
        protected virtual Task<bool> GeneralAudit(AuditEvent evt, EventEntry entry, AuditLog auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != "Update")
            {
                auditEntity.Table = entry.Table;
                auditEntity.PrimaryKey = JsonSerializer.Serialize(entry.PrimaryKey);
                auditEntity.AuditDate = DateTime.UtcNow;
                auditEntity.AuditUserLogin = evt.Environment.CustomFields["UserLogin"].ToString();
                auditEntity.AuditAction = entry.Action; // Insert, Update, Delete
                switch (entry.Action)
                {
                    case "Update":
                        auditEntity.AuditChanges = JsonSerializer.Serialize(entry.Changes);
                        break;
                    case "Insert":
                        auditEntity.AuditChanges = JsonSerializer.Serialize(entry.ColumnValues);
                        break;
                    case "Delete":
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
        /// Dedicates the audit.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="auditEntity">The audit entity.</param>
        /// <returns>True if a change is logged in audit table.</returns>
        protected virtual async Task<bool> DedicatedAudit(AuditEvent evt, EventEntry entry, AuditEntity auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != "Update")
            {
                // Retrieve the associated entity's id
                auditEntity.EntityId = auditEntity.Id.ToString();

                // Reset to zero to avoid entity insertion error
                auditEntity.Id = 0;

                var auditLinkedEntityData = new List<AuditLinkedEntityData>();
                foreach (var linkedEntityPropertyIdentifier in auditEntity.GetType().GetProperties().Where(p => p.GetCustomAttribute<AuditLinkedEntityPropertyIdentifierAttribute>() != null))
                {
                    var linkedEntityType = linkedEntityPropertyIdentifier.GetCustomAttribute<AuditLinkedEntityPropertyIdentifierAttribute>().LinkedEntityType;
                    auditLinkedEntityData.Add(new AuditLinkedEntityData(linkedEntityType.Name, linkedEntityPropertyIdentifier.Name, linkedEntityPropertyIdentifier.GetValue(auditEntity, null).ToString()));
                }

                auditEntity.LinkedEntities = auditLinkedEntityData.Count != 0 ? JsonSerializer.Serialize(auditLinkedEntityData) : null;

                var entityEntry = entry.GetEntry();
                if (entry.Action == "Insert")
                {
                    // Load all unloaded direct references of the entity before filling specific properties
                    var loadReferencesTasks = entityEntry.References.Where(n => !n.IsLoaded).Select(n => n.LoadAsync());
                    await Task.WhenAll(loadReferencesTasks);
                }

                auditEntity.FillSpecificProperties(entityEntry.Entity);

                auditEntity.AuditDate = DateTime.UtcNow;
                auditEntity.AuditUserLogin = evt.Environment.CustomFields["UserLogin"].ToString();
                auditEntity.AuditAction = entry.Action; // Insert, Update, Delete
                switch (entry.Action)
                {
                    case "Update":
                        await this.SetUpdateAuditChanges(auditEntity, entityEntry, entry.Changes);
                        break;
                    case "Insert":
                    case "Delete":
                        auditEntity.AuditChanges = JsonSerializer.Serialize(entry.ColumnValues);
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task SetUpdateAuditChanges(AuditEntity auditEntity, EntityEntry entityEntry, IReadOnlyList<EventEntryChange> changes)
        {
            var auditChanges = new List<AuditChange>();
            var auditLinkedEntityProperties = auditEntity
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute<AuditLinkedEntityPropertyAttribute>() is not null)
                .ToList();

            foreach (var change in changes)
            {
                var auditLinkedEntityProperty = auditLinkedEntityProperties
                    .FirstOrDefault(x => x.GetCustomAttribute<AuditLinkedEntityPropertyAttribute>().LinkedEntityPropertyIdentifier.Equals(change.ColumnName));

                if (auditLinkedEntityProperty is null)
                {
                    auditChanges.Add(new AuditChange(
                        change.ColumnName,
                        change.OriginalValue,
                        change.OriginalValue.ToString(),
                        change.NewValue,
                        change.NewValue.ToString()));
                    continue;
                }

                var auditLinkedEntityPropertyAttribute = auditLinkedEntityProperty.GetCustomAttribute<AuditLinkedEntityPropertyAttribute>();
                var auditLinkedEntityPropertyReference = entityEntry.References.FirstOrDefault(x => x.Metadata.ClrType == auditLinkedEntityPropertyAttribute.LinkedEntityType)
                    ?? throw new BadBiaFrameworkUsageException($"Unable to find any reference of type {auditLinkedEntityPropertyAttribute.LinkedEntityType} into {entityEntry.Entity.GetType()}");

                if (!auditLinkedEntityPropertyReference.IsLoaded)
                {
                    await auditLinkedEntityPropertyReference.LoadAsync();
                }

                var linkedEntity = auditLinkedEntityPropertyReference.TargetEntry.Entity;
                var linkedEntityPropertyDisplayPropertyInfo = linkedEntity.GetType().GetProperty(auditLinkedEntityPropertyAttribute.LinkedEntityPropertyDisplay)
                    ?? throw new BadBiaFrameworkUsageException($"Unable to find property {auditLinkedEntityPropertyAttribute.LinkedEntityPropertyDisplay} into {linkedEntity.GetType()}");

                var newDisplay = linkedEntityPropertyDisplayPropertyInfo.GetValue(linkedEntity, null).ToString();
                auditEntity.GetType().GetProperty(auditLinkedEntityProperty.Name).SetValue(auditEntity, newDisplay, null);

                var oldDisplay = await this.RetrieveOldDisplayChangeFromPreviousAudits(auditEntity, change.ColumnName)
                    ?? await this.RetrieveOldDisplayChangeFromPreviousEntity(linkedEntity.GetType(), change.OriginalValue, auditLinkedEntityPropertyAttribute.LinkedEntityPropertyDisplay);

                auditChanges.Add(new AuditChange(
                    change.ColumnName,
                    change.OriginalValue,
                    oldDisplay,
                    change.NewValue,
                    newDisplay));
            }

            auditEntity.AuditChanges = JsonSerializer.Serialize(auditChanges);
        }

        private async Task<string> RetrieveOldDisplayChangeFromPreviousEntity(Type entityType, object identifierValue, string displayPropertyName)
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

            var identifierProperty = primaryKey.Properties[0];
            var identifierPropertyName = identifierProperty.Name;
            var identifierPropertyType = identifierProperty.ClrType;

            var identifierKey = ConvertKey(identifierValue, identifierPropertyType);
            var entitySet = unitOfWork.RetrieveSet(entityType);
            var entityExpressionParameter = Expression.Parameter(entityType, "e");

            // EF.Property<PK>(e, identifierPropertyName) == identifierKey
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
                entitySet.Expression,
                identifierKeyPredicateExpression);

            // projection EF.Property<object>(e, displayPropertyName)
            var displayPropertyExpression = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                [typeof(object)],
                entityExpressionParameter,
                Expression.Constant(displayPropertyName));

            var displayPropertySelectorExpression = Expression.Lambda(displayPropertyExpression, entityExpressionParameter);

            var displayPropertySelectExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                [entityType, typeof(object)],
                identifierKeyWhereExpression,
                displayPropertySelectorExpression);

            // query
            var query = entitySet.Provider.CreateQuery<object>(displayPropertySelectExpression);

            // FirstOrDefaultAsync
            var firstOrDefaultAsync = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == nameof(EntityFrameworkQueryableExtensions.FirstOrDefaultAsync))
                .MakeGenericMethod(typeof(object));

            var task = (Task<object>)firstOrDefaultAsync.Invoke(null, [query, CancellationToken.None])!;
            var result = await task.ConfigureAwait(false);

            return result?.ToString();
        }

        private static object ConvertKey(object rawKey, Type targetType)
        {
            if (rawKey == null)
            {
                return null;
            }

            var src = rawKey.GetType();

            if (targetType.IsAssignableFrom(src))
            {
                return rawKey;
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, rawKey.ToString()!);
            }

            if (targetType == typeof(Guid))
            {
                return rawKey is Guid g ? g : Guid.Parse(rawKey.ToString()!);
            }

            var t = Nullable.GetUnderlyingType(targetType) ?? targetType;
            return Convert.ChangeType(rawKey, t, System.Globalization.CultureInfo.InvariantCulture);
        }

        private async Task<string> RetrieveOldDisplayChangeFromPreviousAudits(AuditEntity auditEntity, string changePropertyName)
        {
            using var scope = this.serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IQueryableUnitOfWorkNoTracking>();

            await foreach (var rawAuditChanges in unitOfWork
                .RetrieveSet(auditEntity.GetType())
                .Cast<AuditEntity>()
                .AsNoTracking()
                .OrderByDescending(x => x.AuditDate)
                .Where(x => x.EntityId.Equals(auditEntity.EntityId) && x.AuditChanges.Contains(changePropertyName))
                .Select(x => x.AuditChanges)
                .AsAsyncEnumerable())
            {
                var auditChanges = JsonSerializer.Deserialize<List<AuditChange>>(rawAuditChanges);
                var previousAuditChange = auditChanges.FirstOrDefault(x => x.ColumnName == changePropertyName);
                if (previousAuditChange is not null)
                {
                    return previousAuditChange.NewDisplay;
                }
            }

            return null;
        }
    }
}

// <copyright file="BaseAuditFeature.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Features
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Audit.Core;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The Audit Feature.
    /// </summary>
    public class BaseAuditFeature : IAuditFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAuditFeature" /> class.
        /// </summary>
        /// <param name="configuration">the application configuration.</param>
        /// <param name="commonFeaturesConfigurationOptions">The common features configuration options.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        public BaseAuditFeature(IConfiguration configuration, IOptions<CommonFeatures> commonFeaturesConfigurationOptions, IServiceProvider serviceProvider)
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
                global::Audit.Core.Configuration.AuditDisabled = false;

                // Log some Audit in dedicated table and all other in AuditLog
                Audit.Core.Configuration.Setup()
                    .UseEntityFramework(_ => _
                        .AuditTypeMapper(type => this.AuditTypeMapper(type))
                        .AuditEntityAction<IAudit>((evt, entry, audit) =>
                        {
                            if (evt.Environment.Exception != null)
                            {
                                return Task.FromResult(false);
                            }

                            return audit switch
                            {
                                AuditLog auditLog => this.GeneralAudit(evt, entry, auditLog),
                                AuditEntity auditEntity => this.DedicatedAudit(evt, entry, auditEntity),
                                _ => throw new Common.Exceptions.BadBiaFrameworkUsageException($"Unknown audit of type {audit.GetType()}"),
                            };
                        })
                        .IgnoreMatchedProperties(t => t.Name == "AuditLog")); // do not copy properties for generic audit
            }
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
            switch (type.Name)
            {
                case "User":
                    return typeof(UserAudit);
                default:
                    return typeof(AuditLog);
            }
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
        /// Dedicateds the audit.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="auditEntity">The audit entity.</param>
        /// <returns>True if a change is logged in audit table.</returns>
        protected virtual Task<bool> DedicatedAudit(AuditEvent evt, EventEntry entry, AuditEntity auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != "Update")
            {
                auditEntity.EntityId = entry.PrimaryKey.First().Value.ToString();
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
    }
}

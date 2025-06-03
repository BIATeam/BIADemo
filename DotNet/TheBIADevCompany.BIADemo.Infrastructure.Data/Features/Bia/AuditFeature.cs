// <copyright file="AuditFeature.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Features.Bia
{
    using System;
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
    public class AuditFeature : IAuditFeature
    {
        private readonly bool isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditFeature" /> class.
        /// </summary>
        /// <param name="configuration">the application configuration.</param>
        /// <param name="commonFeaturesConfigurationOptions">The common features configuration options.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        public AuditFeature(IConfiguration configuration, IOptions<CommonFeatures> commonFeaturesConfigurationOptions, IServiceProvider serviceProvider)
        {
            Audit.Core.Configuration.AuditDisabled = true;
            AuditConfiguration auditConfiguration = commonFeaturesConfigurationOptions.Value.AuditConfiguration;
            this.isActive = auditConfiguration?.IsActive == true;

            // Audit
            if (this.isActive)
            {
                this.UseAuditFeatures(serviceProvider);
                Audit.Core.Configuration.AuditDisabled = false;

                // Log some Audit in dedicated table and all other in AuditLog
                Audit.Core.Configuration.Setup()
                    .UseEntityFramework(_ => _
                        .AuditTypeMapper(type => AuditTypeMapper(type))
                        .AuditEntityAction<IAuditEntity>((evt, entry, auditEntity) =>
                        {
                            if (evt.Environment.Exception != null)
                            {
                                return Task.FromResult(false);
                            }

                            if (auditEntity.GetType() == typeof(AuditLog))
                            {
                                return GeneralAudit(evt, entry, (AuditLog)auditEntity);
                            }
                            else
                            {
                                return DedicatedAudit(evt, entry, auditEntity);
                            }
                        })
                        .IgnoreMatchedProperties(t => t.Name == "AuditLog")); // do not copy properties for generic audit

#pragma warning disable S125 // Sections of code should not be commented out

                // Log Audit in dedicated table
                // Audit.Core.Configuration.Setup()
                //    .UseEntityFramework(_ => _
                //        .AuditTypeNameMapper(typeName => typeName + "Audit")
                //        .AuditEntityAction<IAuditEntity>((evt, entry, auditEntity) =>
                //        {
                //            return DedicatedAudit(evt, entry, auditEntity);
                //        })
                //    );

                // Log all Audit in AuditLog table
                // Audit.Core.Configuration.Setup()
                //    .UseEntityFramework(_ => _
                //        .AuditTypeMapper(t => typeof(AuditLog))
                //        .AuditEntityAction<AuditLog>((evt, entry, auditEntity) =>
                //        {
                //            return GeneralAudit(evt, entry, auditEntity);
                //        })
                //        .IgnoreMatchedProperties(true)
                //    );
#pragma warning restore S125 // Sections of code should not be commented out
            }
        }

        /// <summary>
        /// Configure the Audit feature in order to retrieve
        /// the current (associated to the request) user.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider.</param>
        public void UseAuditFeatures(IServiceProvider serviceProvider)
        {
            if (this.isActive)
            {
                Audit.Core.Configuration.AddOnSavingAction(scope =>
                {
                    BiaClaimsPrincipal principal = serviceProvider.GetRequiredService<IPrincipal>() as BiaClaimsPrincipal;
                    scope.Event.Environment.CustomFields["UserLogin"] = principal.Identity.Name;
                });
            }
        }

        private static Type AuditTypeMapper(Type type)
        {
            switch (type.Name)
            {
                case "User":
                    return typeof(UserAudit);
                default:
                    return typeof(AuditLog);
            }
        }

        private static Task<bool> GeneralAudit(AuditEvent evt, EventEntry entry, AuditLog auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != "Update")
            {
                // auditEntity is of IAudit type
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

        private static Task<bool> DedicatedAudit(AuditEvent evt, EventEntry entry, IAuditEntity auditEntity)
        {
            if (entry.Changes?.Count > 0 || entry.Action != "Update")
            {
                // auditEntity is of IAudit type
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

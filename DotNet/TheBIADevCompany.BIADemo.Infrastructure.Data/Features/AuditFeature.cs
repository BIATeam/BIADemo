// <copyright file="AuditFeature.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Features
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Audit.Core;
    using Audit.EntityFramework;
    using Audit.EntityFramework.ConfigurationApi;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Audit.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The Audit Feature.
    /// </summary>
    public class AuditFeature : IAuditFeature
    {
        private readonly bool isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditFeature"/> class.
        /// </summary>
        /// <param name="configuration">the application configuration.</param>
        /// <param name="auditConfigurationOptions">the audit configuration.</param>
        public AuditFeature(IConfiguration configuration, IOptions<AuditConfiguration> auditConfigurationOptions)
        {
            Audit.Core.Configuration.AuditDisabled = true;
            AuditConfiguration auditConfiguration = auditConfigurationOptions.Value;
            this.isActive = auditConfiguration?.IsActive == true;

            // Audit
            if (this.isActive)
            {
                Audit.Core.Configuration.AuditDisabled = false;
                /*IIncludeConfigurator<DataContext> configurator = Audit.EntityFramework.Configuration.Setup()
                    .ForContext<DataContext>(config => config
                        .IncludeEntityObjects(false)
                        .AuditEventType("{context}:{database}"))
                        .UseOptIn();*/

                // Replace by the table you want to Audit :
                // configurator.Include<Plane>().Include<Airport>()

                // Begin BIADemo
                //configurator.Include<Plane>().Include<Airport>();
                //configurator.Include<User>();

                // End BIADemo

                //Audit.Core.Configuration.Setup().UseSqlServer(config => config
                //    .ConnectionString(configuration.GetConnectionString("BIADemoDatabase"))
                //    .Schema("dbo")
                //    .TableName("Events")
                //    .IdColumnName("Id")
                //    .JsonColumnName("JsonData")
                //    .LastUpdatedColumnName("LastUpdatedDate")
                //    .CustomColumn("EventType", ev => ev.EventType)
                //    .CustomColumn("UserId", ev => ev.Environment.CustomFields["UserId"]));

                //Audit.EntityFramework.Configuration.Setup()
                //    .ForContext<DataContext>(config => config
                //        .ForEntity<User>(_ => _
                //            .Ignore(user => user.LastLoginDate)));

                //Audit.Core.Configuration.Setup()
                //    .UseEntityFramework(ef => ef
                //        .AuditTypeExplicitMapper(m => m
                //            .Map<User, AuditUser>()
                //            .Map<Airport, AirportAudit>()
                //            .AuditEntityAction<IAudit>((evt, entry, auditEntity) =>
                //            {
                //                auditEntity.AuditDate = DateTime.UtcNow;
                //                auditEntity.UserId = (int) evt.Environment.CustomFields["UserId"];
                //                auditEntity.AuditAction = entry.Action; // Insert, Update, Delete
                //                return Task.FromResult(true);
                //            })
                //        )
                //    );

                Audit.Core.Configuration.Setup()
                    .UseEntityFramework(x => x
                        .AuditTypeNameMapper(typeName => typeName + "Audit")
                        .AuditEntityAction<IAuditEntity>((evt, entry, auditEntity) =>
                        {
                            if (entry.Changes?.Count > 0)
                            {
                                // auditEntity is of IAudit type
                                auditEntity.AuditDate = DateTime.UtcNow;
                                auditEntity.AuditUserLogin = evt.Environment.CustomFields["UserLogin"].ToString();
                                auditEntity.AuditAction = entry.Action; // Insert, Update, Delete
                                auditEntity.AuditChanges = JsonSerializer.Serialize(entry.Changes);
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }));
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
                    BIAClaimsPrincipal principal = serviceProvider.GetRequiredService<IPrincipal>() as BIAClaimsPrincipal;
                    scope.Event.Environment.CustomFields["UserLogin"] = principal.Identity.Name;
                });
            }
        }
    }
}

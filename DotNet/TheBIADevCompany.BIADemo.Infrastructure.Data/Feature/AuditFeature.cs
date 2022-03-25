// <copyright file="AuditFeature.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Feature
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using Audit.Core;
    using Audit.EntityFramework.ConfigurationApi;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The Audit Feature.
    /// </summary>
    public class AuditFeature
    {
        private readonly bool isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditFeature"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">the application configuration.</param>
        /// <param name="auditConfigurationOptions">the audit configuration.</param>
        public AuditFeature(ILogger<AuditFeature> logger, IConfiguration configuration, IOptions<AuditConfiguration> auditConfigurationOptions)
        {
            Audit.Core.Configuration.AuditDisabled = true;
            AuditConfiguration auditConfiguration = auditConfigurationOptions.Value;
            this.isActive = auditConfiguration?.IsActive == true;

            // Audit
            if (this.isActive)
            {
                Audit.Core.Configuration.AuditDisabled = false;
                IIncludeConfigurator<DataContext> configurator = Audit.EntityFramework.Configuration.Setup()
                    .ForContext<DataContext>(config => config
                        .IncludeEntityObjects(false)
                        .AuditEventType("{context}:{database}"))
                        .UseOptIn();

                // configurator.Include<Plane>().Include<Airport>();

                // Begin BIADemo
                configurator.Include<Plane>().Include<Airport>();

                // End BIADemo

                Audit.Core.Configuration.Setup().UseSqlServer(config => config
                    .ConnectionString(configuration.GetConnectionString("BIADemoDatabase"))
                    .Schema("dbo")
                    .TableName("Events")
                    .IdColumnName("Id")
                    .JsonColumnName("JsonData")
                    .LastUpdatedColumnName("LastUpdatedDate")
                    .CustomColumn("EventType", ev => ev.EventType)
                    .CustomColumn("UserId", ev => ev.Environment.CustomFields["UserId"]));
            }
        }

        /// <summary>
        /// Configure the Audit feature in order to retrieve
        /// the current (associated to the request) user.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider.</param>
        public void UseAuditFeatures(IServiceProvider serviceProvider) {
            if (this.isActive)
            {
                Audit.Core.Configuration.AddOnSavingAction(scope =>
                {
                    BIAClaimsPrincipal principal = serviceProvider.GetRequiredService<IPrincipal>() as BIAClaimsPrincipal;
                    scope.Event.Environment.CustomFields["UserId"] = principal.GetUserId();
                });
            }
        }
    }
}

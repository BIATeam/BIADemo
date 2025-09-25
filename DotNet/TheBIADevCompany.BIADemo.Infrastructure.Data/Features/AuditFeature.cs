// <copyright file="AuditFeature.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Features
{
    using System;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Infrastructure.Data.Features;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    // End BIADemo

    /// <summary>
    /// The Audit Feature.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="commonFeaturesConfigurationOptions">The common featyres configuration options.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public class AuditFeature(IConfiguration configuration, IOptions<CommonFeatures> commonFeaturesConfigurationOptions, IServiceProvider serviceProvider) : BaseAuditFeature(configuration, commonFeaturesConfigurationOptions, serviceProvider)
    {
        /// <summary>
        /// Audits the type mapper.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type of the Audit entity.</returns>
        public override Type AuditTypeMapper(Type type)
        {
            return type.Name switch
            {
                // Begin BIADemo
                nameof(Plane) => typeof(PlaneAudit),
                nameof(Engine) => typeof(EngineAudit),
                nameof(PlaneAirport) => typeof(PlaneAirportAudit),

                // End BIADemo
                _ => base.AuditTypeMapper(type),
            };
        }
    }
}

// <copyright file="AuditFeature.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Features
{
    using System;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Infrastructure.Data.Features;
    using Microsoft.Extensions.Options;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The Audit Feature.
    /// </summary>
    /// <param name="commonFeaturesConfigurationOptions">The common features configuration options.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public class AuditFeature(IOptions<CommonFeatures> commonFeaturesConfigurationOptions, IServiceProvider serviceProvider) : BaseAuditFeature(commonFeaturesConfigurationOptions, serviceProvider)
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
                // BIAToolKit - Begin AuditTypeMapper
                // Begin BIAToolKit Generation Ignore
                // BIAToolKit - Begin Partial AuditTypeMapper Plane
                nameof(Plane) => typeof(PlaneAudit),

                // BIAToolKit - End Partial AuditTypeMapper Plane
                // End BIAToolKit Generation Ignore
                // BIAToolKit - End AuditTypeMapper

                // Begin BIADemo
                nameof(Engine) => typeof(EngineAudit),
                nameof(PlaneAirport) => typeof(PlaneAirportAudit),

                // End BIADemo
                nameof(User) => typeof(UserAudit),
                _ => base.AuditTypeMapper(type),
            };
        }
    }
}

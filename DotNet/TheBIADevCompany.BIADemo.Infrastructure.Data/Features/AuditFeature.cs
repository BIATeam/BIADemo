// <copyright file="AuditFeature.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Features
{
    using System;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Infrastructure.Data.Features;
    using Microsoft.Extensions.Options;

    // BIAToolKit - Begin AuditTypeMapperUsing
    // Begin BIAToolKit Generation Ignore
#if BIA_FRONT_FEATURE
    // BIAToolKit - Begin Partial AuditTypeMapperUsing Fleet
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    // BIAToolKit - End Partial AuditTypeMapperUsing Fleet
#endif

    // End BIAToolKit Generation Ignore
    // BIAToolKit - End AuditTypeMapperUsing
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
#endif

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
#if BIA_FRONT_FEATURE
                nameof(Plane) => typeof(PlaneAudit),
#endif
                // BIAToolKit - End Partial AuditTypeMapper Plane
                // End BIAToolKit Generation Ignore
                // BIAToolKit - End AuditTypeMapper
#if BIA_FRONT_FEATURE
                // Begin BIADemo
                nameof(Engine) => typeof(EngineAudit),
                nameof(PlaneAirport) => typeof(PlaneAirportAudit),
                nameof(PlanePlaneType) => typeof(PlanePlaneTypeAudit),

                // End BIADemo

                nameof(User) => typeof(UserAudit),
#endif
                _ => base.AuditTypeMapper(type),
            };
        }
    }
}

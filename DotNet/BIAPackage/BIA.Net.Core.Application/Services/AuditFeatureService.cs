// <copyright file="AuditFeatureService.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The service for Audit feature.
    /// </summary>
    public class AuditFeatureService : IAuditFeatureService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IAuditFeature auditFeature;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditFeatureService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="auditFeature">The audit feature.</param>
        public AuditFeatureService(IServiceProvider serviceProvider, IAuditFeature auditFeature)
        {
            this.serviceProvider = serviceProvider;
            this.auditFeature = auditFeature;
        }

        /// <inheritdoc/>
        public void EnableAuditFeatures()
        {
            this.auditFeature.UseAuditFeatures(this.serviceProvider);
        }
    }
}

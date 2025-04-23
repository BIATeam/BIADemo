// <copyright file="IAuditFeatureService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    /// <summary>
    /// Interface for audit feature service.
    /// </summary>
    public interface IAuditFeatureService
    {
        /// <summary>
        /// Enable the audit feature.
        /// </summary>
        void EnableAuditFeatures();
    }
}
// <copyright file="IAuditFeature.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;

    /// <summary>
    /// IAuditFeature.
    /// </summary>
    public interface IAuditFeature
    {
        /// <summary>
        /// Uses the audit features.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        void UseAuditFeatures(IServiceProvider serviceProvider);
    }
}

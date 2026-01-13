// <copyright file="IAuditFeature.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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

        /// <summary>
        /// Audits the type mapper.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type of the Audit entity.</returns>
        Type AuditTypeMapper(Type type);
    }
}

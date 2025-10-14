// <copyright file="IAuditEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Interface for dedicated audit entity.
    /// </summary>
    public interface IAuditEntity : IAudit
    {
        /// <summary>
        /// Fill specific properties of the audit based on the current audited <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="T">Audited entity type.</typeparam>
        /// <param name="entity">Audited entity.</param>
        void FillSpecificProperties<T>(T entity);
    }
}
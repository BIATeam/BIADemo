// <copyright file="AuditEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Dedicated audit entity.
    /// </summary>
    public class AuditEntity : BaseAudit
    {
        /// <summary>
        /// The audited entity id.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// JSON value of <see cref="AuditLinkedEntityData"/> collection.
        /// </summary>
        public string LinkedEntities { get; set; }

        /// <summary>
        /// Fill specific properties of the audit based on the current audited <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="TEntity">Audited entity type.</typeparam>
        /// <param name="entity">Audited entity.</param>
        public virtual void FillSpecificProperties<TEntity>(TEntity entity)
        {
        }
    }
}

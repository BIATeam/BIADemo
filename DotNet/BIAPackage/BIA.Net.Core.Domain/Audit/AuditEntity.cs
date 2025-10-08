// <copyright file="AuditEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Dedicated audit entity.
    /// </summary>
    /// <typeparam name="TEntity">Audited entity type.</typeparam>
    public class AuditEntity<TEntity> : BaseAudit, IAuditEntity
    {
        /// <inheritdoc/>
        public string EntityId { get; set; }

        /// <inheritdoc/>
        public string LinkedEntities { get; set; }

        /// <inheritdoc/>
        public void FillSpecificProperties<T>(T entity)
        {
            if (entity is not TEntity typedEntity)
            {
                return;
            }

            this.FillSpecificProperties(typedEntity);
        }

        /// <summary>
        /// Fill specific properties of the audit based on the current audited <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Audited entity.</param>
        protected virtual void FillSpecificProperties(TEntity entity)
        {
        }
    }
}

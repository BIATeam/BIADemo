// <copyright file="AuditEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Dedicated audit for <typeparamref name="TEntity"/> with <typeparamref name="TAuditKey"/> audit key type.
    /// </summary>
    /// <typeparam name="TEntity">Audited entity type.</typeparam>
    /// <typeparam name="TAuditKey">Audit key type.</typeparam>
    public abstract class AuditEntity<TEntity, TAuditKey> : BaseAudit<TAuditKey>, IAuditEntity
    {
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
        /// Fill specific properties of the audit based on the current audited <typeparamref name="TEntity"/> <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Audited entity.</param>
        protected virtual void FillSpecificProperties(TEntity entity)
        {
        }
    }
}

// <copyright file="IAuditKeyedEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Interface for dedicated audit of an entity with <typeparamref name="TKey"/> Id type.
    /// </summary>
    /// <typeparam name="TKey">Tha audited entity key type.</typeparam>
    public interface IAuditKeyedEntity<out TKey> : IAuditEntity
    {
        /// <summary>
        /// The <typeparamref name="TEntity"/> ID.
        /// </summary>
        TKey Id { get; }
    }
}

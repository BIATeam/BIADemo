// <copyright file="IAuditKeyedEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Interface for dedicated audit of an entity with key type <typeparamref name="TEntityKey"/>.
    /// </summary>
    /// <typeparam name="TEntityKey">Tha audited entity key type.</typeparam>
    public interface IAuditKeyedEntity<out TEntityKey> : IAuditEntity
    {
        /// <summary>
        /// The <typeparamref name="TEntity"/> Id key.
        /// </summary>
        TEntityKey Id { get; }
    }
}

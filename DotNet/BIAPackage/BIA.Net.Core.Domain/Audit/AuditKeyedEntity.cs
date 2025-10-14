// <copyright file="AuditKeyedEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using System.ComponentModel.DataAnnotations;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// Dedicated audit for <typeparamref name="TEntity"/> of key type <typeparamref name="TEntityKey"/> with <typeparamref name="TAuditKey"/> audit key type.
    /// </summary>
    /// <typeparam name="TEntity">Audited entity type.</typeparam>
    /// <typeparam name="TEntityKey">Audited entity key type.</typeparam>
    /// <typeparam name="TAuditKey">Audit key type.</typeparam>
    public abstract class AuditKeyedEntity<TEntity, TEntityKey, TAuditKey> : AuditEntity<TEntity, TAuditKey>, IAuditKeyedEntity<TEntityKey>
        where TEntity : IEntity<TEntityKey>
    {
        /// <inheritdoc/>
        [Required]
        public TEntityKey Id { get; set; }
    }
}

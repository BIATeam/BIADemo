// <copyright file="AuditEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using System.ComponentModel.DataAnnotations;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// Dedicated audit for <typeparamref name="TEntity"/> (Id type=<typeparamref name="TKey"/>) with <typeparamref name="TAuditKey"/> audit Id type.
    /// </summary>
    /// <typeparam name="TEntity">Audited entity type.</typeparam>
    /// <typeparam name="TKey">Audited entity key type.</typeparam>
    /// <typeparam name="TAuditKey">Audit key type.</typeparam>
    public abstract class AuditKeyedEntity<TEntity, TKey, TAuditKey> : AuditEntity<TEntity, TAuditKey>, IAuditKeyedEntity<TKey>
        where TEntity : IEntity<TKey>
    {
        /// <inheritdoc/>
        [Required]
        public TKey Id { get; set; }
    }
}

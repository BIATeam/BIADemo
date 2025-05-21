// <copyright file="BaseEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity
{
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The base class for Entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public TKey Id { get; set; }
    }
}

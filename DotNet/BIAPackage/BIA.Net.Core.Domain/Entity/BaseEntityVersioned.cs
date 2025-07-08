// <copyright file="BaseEntityVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity
{
    using System.ComponentModel.DataAnnotations;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The base class for Entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public abstract class BaseEntityVersioned<TKey> : BaseEntity<TKey>, IEntityVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}

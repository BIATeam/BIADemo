// <copyright file="BaseEntityVersionedFixable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity
{
    using System;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The base class for Entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public abstract class BaseEntityVersionedFixable<TKey> : BaseEntityVersioned<TKey>, IEntityFixable
    {
        /// <summary>
        /// Gets or sets the is fixed.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the fixed date.
        /// </summary>
        public DateTime? FixedDate { get; set; }
    }
}

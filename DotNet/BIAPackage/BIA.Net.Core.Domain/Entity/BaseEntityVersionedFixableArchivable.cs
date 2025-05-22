// <copyright file="BaseEntityVersionedFixableArchivable.cs" company="BIA">
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
    public abstract class BaseEntityVersionedFixableArchivable<TKey> : BaseEntityVersionedFixable<TKey>, IEntityArchivable
    {
        /// <summary>
        /// Gets or sets the is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the archived date.
        /// </summary>
        public DateTime? ArchivedDate { get; set; }
    }
}

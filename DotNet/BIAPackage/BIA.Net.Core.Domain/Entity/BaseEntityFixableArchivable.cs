// <copyright file="BaseEntityFixableArchivable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity
{
    using System;
    using BIA.Net.Core.Domain.Entity.Interface;
    using global::Audit.EntityFramework;

    /// <summary>
    /// The base class for Entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public abstract class BaseEntityFixableArchivable<TKey> : BaseEntityFixable<TKey>, IEntityArchivable
    {
        /// <summary>
        /// Gets or sets the is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the archived date.
        /// </summary>
        [AuditIgnore]
        public DateTime? ArchivedDate { get; set; }
    }
}

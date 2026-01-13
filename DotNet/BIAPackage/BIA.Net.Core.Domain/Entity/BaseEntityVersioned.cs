// <copyright file="BaseEntityVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity
{
    using BIA.Net.Core.Domain.Entity.Interface;
    using global::Audit.EntityFramework;

    /// <summary>
    /// The base class for Entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public abstract class BaseEntityVersioned<TKey> : BaseEntity<TKey>, IEntityVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [AuditIgnore]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [AuditIgnore]
        public uint RowVersionXmin { get; set; }
    }
}

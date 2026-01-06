// <copyright file="VersionedTable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain
{
    using BIA.Net.Core.Domain.Entity.Interface;
    using global::Audit.EntityFramework;

    /// <summary>
    /// The versioned table class used to apply RowVersion on all table.
    /// </summary>
    public class VersionedTable : IEntityVersioned
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

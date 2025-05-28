// <copyright file="AuditLog.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The airport entity.
    /// </summary>
    public class AuditLog : AuditEntity, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the PrimaryKey.
        /// </summary>
        public string PrimaryKey { get; set; }
    }
}

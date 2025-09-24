// <copyright file="AuditLog.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The audit log entity.
    /// </summary>
    public class AuditLog : BaseAudit
    {
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

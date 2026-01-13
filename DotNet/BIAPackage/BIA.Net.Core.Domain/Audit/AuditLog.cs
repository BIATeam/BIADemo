// <copyright file="AuditLog.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The audit log entity.
    /// </summary>
    public sealed class AuditLog : BaseAudit<int>
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

// <copyright file="AuditEntity.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using System;

    /// <summary>
    /// The user entity.
    /// </summary>
    public class AuditEntity : VersionedTable, IAuditEntity
    {
        /// <summary>
        /// Gets or sets the AuditDate.
        /// </summary>
        public DateTime AuditDate { get; set; }

        /// <summary>
        /// Gets or sets the AuditAction.
        /// </summary>
        public string AuditAction { get; set; }

        /// <summary>
        /// Gets or sets the Audit Changes.
        /// </summary>
        public string AuditChanges { get; set; }

        /// <summary>
        /// Gets or sets the Audit User login.
        /// </summary>
        public string AuditUserLogin { get; set; }
    }
}
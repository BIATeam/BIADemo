// <copyright file="AuditEntity.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using System;
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The audit entity.
    /// </summary>
    public abstract class BaseAudit : BaseEntityVersioned<int>, IAudit
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
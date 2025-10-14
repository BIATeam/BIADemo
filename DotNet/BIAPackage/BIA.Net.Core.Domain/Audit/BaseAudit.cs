// <copyright file="BaseAudit.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The audit entity with <typeparamref name="TKey"/> key type.
    /// </summary>
    /// <typeparam name="TKey">Audit key type.</typeparam>
    public abstract class BaseAudit<TKey> : IAudit
    {
        /// <summary>
        /// The audit id.
        /// </summary>
        [Key]
        public TKey AuditId { get; set; }

        /// <inheritdoc/>
        [Required]
        public DateTime AuditDate { get; set; }

        /// <inheritdoc/>
        [Required]
        public string AuditAction { get; set; }

        /// <inheritdoc/>
        [Required]
        public string AuditChanges { get; set; }

        /// <inheritdoc/>
        [Required]
        public string AuditUserLogin { get; set; }
    }
}
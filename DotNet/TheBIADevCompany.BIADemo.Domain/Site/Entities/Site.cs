// <copyright file="Site.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Site.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BIA.Net.Core.Domain.User.Entities;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    // End BIADemo

    /// <summary>
    /// The site entity.
    /// </summary>
    public class Site : BaseEntityTeam
    {
        /// <summary>
        /// Add row version timestamp in table Site.
        /// </summary>
        [Timestamp]
        [Column("RowVersion")]
        public byte[] RowVersionSite { get; set; }

        // Begin BIADemo

        /// <summary>
        /// Gets or sets the Maintenance contracts.
        /// </summary>
        public virtual ICollection<MaintenanceContract> MaintenanceContracts { get; set; }

        // End BIADemo
    }
}
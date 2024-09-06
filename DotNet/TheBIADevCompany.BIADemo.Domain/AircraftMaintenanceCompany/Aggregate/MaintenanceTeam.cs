// BIADemo only
// <copyright file="MaintenanceTeam.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The AircraftMaintenanceCompany entity.
    /// </summary>
    public class MaintenanceTeam : Team
    {
        /// <summary>
        /// Gets or sets the aircraft maintenance company.
        /// </summary>
        public virtual AircraftMaintenanceCompany AircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the aircraft maintenance company id.
        /// </summary>
        public int AircraftMaintenanceCompanyId { get; set; }

        /// <summary>
        /// Add row version timestamp in table Site.
        /// </summary>
        [Timestamp]
        [Column("RowVersion")]
        public byte[] RowVersionAircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team active indicator.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
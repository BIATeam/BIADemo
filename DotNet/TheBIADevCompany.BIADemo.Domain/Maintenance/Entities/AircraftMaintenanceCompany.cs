// BIADemo only
// <copyright file="AircraftMaintenanceCompany.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Audit.EntityFramework;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The AircraftMaintenanceCompany entity.
    /// </summary>
    public class AircraftMaintenanceCompany : BaseEntityTeam
    {
        /// <summary>
        /// Add row version timestamp in table AircraftMaintenanceCompany.
        /// </summary>
        [Column(nameof(IEntityVersioned.RowVersion))]
        [AuditIgnore]
        public byte[] RowVersionAircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Add row version for Postgre in table AircraftMaintenanceCompany.
        /// </summary>
        [Column(nameof(IEntityVersioned.RowVersionXmin))]
        [AuditIgnore]
        public uint RowVersionXminAircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the Maintenance teams.
        /// </summary>
        public virtual ICollection<MaintenanceTeam> MaintenanceTeams { get; set; }

        /// <summary>
        /// Gets or sets the Maintenance contracts.
        /// </summary>
        public virtual ICollection<MaintenanceContract> MaintenanceContracts { get; set; }
    }
}
﻿// BIADemo only
// <copyright file="MaintenanceTeamAirport.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Entities
{
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The entity to link maintenance teams and airports.
    /// </summary>
    public class MaintenanceTeamAirport : VersionedTable
    {
        /// <summary>
        /// Gets or sets the MaintenanceTeam.
        /// </summary>
        public MaintenanceTeam MaintenanceTeam { get; set; }

        /// <summary>
        /// Gets or sets the MaintenanceTeam id.
        /// </summary>
        public int MaintenanceTeamId { get; set; }

        /// <summary>
        /// Gets or sets the Airport.
        /// </summary>
        public Airport Airport { get; set; }

        /// <summary>
        /// Gets or sets the Airport id.
        /// </summary>
        public int AirportId { get; set; }
    }
}

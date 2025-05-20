// BIADemo only
// <copyright file="MaintenanceTeamCountry.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity to link maintenance teams and countries.
    /// </summary>
    public class MaintenanceTeamCountry : VersionedTable
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
        /// Gets or sets the Country.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// Gets or sets the Country id.
        /// </summary>
        public int CountryId { get; set; }
    }
}

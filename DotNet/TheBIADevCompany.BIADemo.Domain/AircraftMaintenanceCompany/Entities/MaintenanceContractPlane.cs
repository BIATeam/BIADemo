// BIADemo only
// <copyright file="MaintenanceContractPlane.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities
{
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The entity PlaneAirport repair site.
    /// </summary>
    public class MaintenanceContractPlane : VersionedTable
    {
        /// <summary>
        /// Gets or sets the MaintenanceContract.
        /// </summary>
        public MaintenanceContract MaintenanceContract { get; set; }

        /// <summary>
        /// Gets or sets the MaintenanceContract id.
        /// </summary>
        public int MaintenanceContractId { get; set; }

        /// <summary>
        /// Gets or sets the Plane.
        /// </summary>
        public Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the Plane id.
        /// </summary>
        public int PlaneId { get; set; }
    }
}

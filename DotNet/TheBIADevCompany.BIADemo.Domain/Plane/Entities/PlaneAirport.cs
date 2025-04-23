// BIADemo only
// <copyright file="PlaneAirport.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity PlaneAirport repair site.
    /// </summary>
    public class PlaneAirport : VersionedTable
    {
        /// <summary>
        /// Gets or sets the Plane.
        /// </summary>
        public Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the Plane id.
        /// </summary>
        public int PlaneId { get; set; }

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

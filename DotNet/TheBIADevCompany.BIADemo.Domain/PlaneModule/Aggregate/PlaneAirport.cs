// BIADemo only
// <copyright file="PlaneAirport.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity conformcertif repair site.
    /// </summary>
    public class PlaneAirport : VersionedTable
    {
        /// <summary>
        /// Gets or sets the conformcertif.
        /// </summary>
        public Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the conformcertif id.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public Airport Airport { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int AirportId { get; set; }
    }
}

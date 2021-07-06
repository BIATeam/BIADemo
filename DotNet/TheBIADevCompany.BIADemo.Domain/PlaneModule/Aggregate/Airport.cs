// BIADemo only
// <copyright file="Airport.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The airport entity.
    /// </summary>
    public class Airport : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the airport.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the City where is the airport.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the list of planes using the airport.
        /// </summary>
        public ICollection<PlaneAirport> ClientPlanes { get; set; }
    }
}

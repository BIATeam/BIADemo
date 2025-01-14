// BIADemo only
// <copyright file="Airport.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain;
    using global::Audit.EntityFramework;

    /// <summary>
    /// The airport entity.
    /// </summary>
    [AuditInclude]
    public class Airport : VersionedTable, IEntity<int>
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
        [AuditIgnore]
        [AuditOverride(null)]
        public string City { get; set; }
    }
}

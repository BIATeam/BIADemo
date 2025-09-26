// BIADemo only
// <copyright file="Airport.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using Audit.EntityFramework;
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The airport entity.
    /// </summary>
    [AuditInclude]
    public class Airport : BaseEntityVersioned<int>
    {
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

// BIADemo only
// <copyright file="AirportAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Entity.Interface;
    using global::Audit.EntityFramework;

    /// <summary>
    /// The airport entity.
    /// </summary>
    public class AirportAudit : AuditEntity, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int AuditId { get; set; }

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
        public string City { get; set; }
    }
}

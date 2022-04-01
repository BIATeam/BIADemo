// BIADemo only
// <copyright file="AirportAudit.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Audit;
    using global::Audit.EntityFramework;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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

// BIADemo only
// <copyright file="Flight.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Entity;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The flight entity.
    /// </summary>
    public class Flight : BaseEntityVersionedFixableArchivable<string>
    {
        /// <summary>
        /// Gets or sets the departure airport.
        /// </summary>
        public Airport DepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets the departur airport id.
        /// </summary>
        public int DepartureAirportId { get; set; }

        /// <summary>
        /// Gets or sets the arrival airport.
        /// </summary>
        public Airport ArrivalAirport { get; set; }

        /// <summary>
        /// Gets or sets the arrival airport id.
        /// </summary>
        public int ArrivalAirportId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual Site Site { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int SiteId { get; set; }
    }
}
// BIADemo only
// <copyright file="Pilot.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Entity;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The pilot entity.
    /// </summary>
    public class Pilot : BaseEntityVersionedFixableArchivable<Guid>
    {
        /// <summary>
        /// Gets or sets the Identification number.
        /// </summary>
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the First Name of the pilot.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name of the pilot.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Birthdate of the pilot.
        /// </summary>
        public string Birthdate { get; set; }

        /// <summary>
        /// Gets or sets the Commercial Pilot License obtention date.
        /// </summary>
        public DateTime CPLDate { get; set; }

        /// <summary>
        /// Gets or sets the Flight Hours.
        /// </summary>
        public int FlightHours { get; set; }

        /// <summary>
        /// Gets or sets the Base Airport.
        /// </summary>
        public Airport BaseAirport { get; set; }

        /// <summary>
        /// Gets or sets the Base Airport id.
        /// </summary>
        public int BaseAirportId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual Site Site { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        public DateTimeOffset FirstFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the last flight date.
        /// </summary>
        public DateTimeOffset? LastFlightDate { get; set; }
    }
}
// BIADemo only
// <copyright file="PilotDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Fleet
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a pilot.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "Site")]
    public class PilotDto : BaseDtoVersionedFixableArchivable<Guid>
    {
        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        [BiaDtoField(Required = true, IsParent = true)]
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the identification number.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birthdate.
        /// </summary>
        [BiaDtoField(Required = false, Type = "date")]
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Gets or sets the CPL date.
        /// </summary>
        [BiaDtoField(Required = true, Type = "date")]
        public DateTime CPLDate { get; set; }

        /// <summary>
        /// Gets or sets the base airport.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Airport")]
        public OptionDto BaseAirport { get; set; }

        /// <summary>
        /// Gets or sets the flight hours.
        /// </summary>
        [BiaDtoField(Required = true)]
        public int FlightHours { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime", AsLocalDateTime = true)]
        public DateTimeOffset FirstFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the last flight date.
        /// </summary>
        [BiaDtoField(Required = false, Type = "datetime", AsLocalDateTime = true)]
        public DateTimeOffset? LastFlightDate { get; set; }
    }
}
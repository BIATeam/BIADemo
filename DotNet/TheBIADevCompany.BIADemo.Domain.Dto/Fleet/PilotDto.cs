// BIADemo only
// <copyright file="PilotDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Fleet
{
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
        /// Gets or sets the Identification Number.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the First Name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        [BiaDtoField(Required = false)]
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Gets or sets the Commercial Pilot License obtention date.
        /// </summary>
        [BiaDtoField(Required = true)]
        public DateTime CPLDate { get; set; }

        /// <summary>
        /// Gets or sets the Base Airport.
        /// </summary>
        [BiaDtoField(Required = false)]
        public OptionDto BaseAirport { get; set; }

        /// <summary>
        /// Gets or sets the flight hours.
        /// </summary>
        [BiaDtoField(Required = false)]
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
// BIADemo only
// <copyright file="FlightDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Fleet
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a flight.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "Site")]
    public class FlightDto : BaseDtoVersionedFixableArchivable<string>
    {
        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        [BiaDtoField(Required = true, IsParent = true)]
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the departure airport.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Airport")]
        public OptionDto DepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets the arrival airport.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Airport")]
        public OptionDto ArrivalAirport { get; set; }
    }
}
// BIADemo only
// <copyright file="PilotDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Fleet
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "Site")]
    public class PilotDto : BaseDtoVersionedFixableArchivable<string>
    {
        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        [BiaDtoField(Required = true, IsParent = true)]
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the msn.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        [BiaDtoField(Required = false)]
        public int FlightHours { get; set; }
    }
}
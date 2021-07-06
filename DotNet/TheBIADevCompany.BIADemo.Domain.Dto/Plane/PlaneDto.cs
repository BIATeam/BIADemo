// BIADemo only
// <copyright file="PlaneDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    public class PlaneDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the first flight date only.
        /// </summary>
        public DateTime FirstFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the first flight time only.
        /// </summary>
        public DateTime FirstFlightTime { get; set; }

        /// <summary>
        /// Gets or sets the last flight date and time.
        /// </summary>
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public SiteDto Site { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports.
        /// </summary>
        public ICollection<OptionDto> ConnectingAirports { get; set; }

        /// <summary>
        /// Gets or sets the  plane type title.
        /// </summary>
        public OptionDto PlaneType { get; set; }
    }
}
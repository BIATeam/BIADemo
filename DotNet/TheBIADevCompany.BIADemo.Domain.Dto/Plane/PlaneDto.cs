// BIADemo only
// <copyright file="PlaneDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    public class PlaneDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        [BIADtoField(Required = true)]
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the last flight date and time.
        /// </summary>
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        [BIADtoField(Type = "time", Required = false)]
        public string SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports.
        /// </summary>
        public ICollection<OptionDto> ConnectingAirports { get; set; }

        /// <summary>
        /// Gets or sets the  plane type title.
        /// </summary>
        [BIADtoField(Required = false)]
        public OptionDto PlaneType { get; set; }
    }
}
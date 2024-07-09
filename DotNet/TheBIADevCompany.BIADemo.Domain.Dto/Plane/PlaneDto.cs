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
    [BiaDtoClass(AncestorTeam = "Site")]
    public class PlaneDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is active.
        /// </summary>
        [BiaDtoField(Required = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the last flight date and time.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = false)]
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        [BiaDtoField(Type = "date", Required = false)]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        [BiaDtoField(Type = "time", Required = false)]
        public string SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        [BiaDtoField(Required = true)]
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the probability.
        /// </summary>
        [BiaDtoField(Required = false)]
        public double? Probability { get; set; }

        /// <summary>
        /// Gets or sets the fuelLevel.
        /// </summary>
        [BiaDtoField(Required = false)]
        public float? FuelLevel { get; set; }

        /// <summary>
        /// Gets or sets the estimated price.
        /// </summary>
        [BiaDtoField(Required = false)]
        public decimal? EstimatedPrice { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        [BiaDtoField(IsParent = true, Required = true)]
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports.
        /// </summary>
        [BiaDtoField(ItemType = "Airport")]
        public ICollection<OptionDto> ConnectingAirports { get; set; }

        /// <summary>
        /// Gets or sets the  plane type title.
        /// </summary>
        [BiaDtoField(ItemType = "PlaneType")]
        public OptionDto PlaneType { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports.
        /// </summary>
        [BiaDtoField(ItemType = "PlaneType")]
        public ICollection<OptionDto> SimilarTypes { get; set; }

        /// <summary>
        /// Gets or sets the  plane type title.
        /// </summary>
        [BiaDtoField(ItemType = "Airport")]
        public OptionDto CurrentAirport { get; set; }
    }
}
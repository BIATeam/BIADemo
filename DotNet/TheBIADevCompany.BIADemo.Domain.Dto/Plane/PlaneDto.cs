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
    public class PlaneDto : FixableDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is active.
        /// </summary>
        [BiaDtoField(Required = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is on maintenance.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool? IsMaintenance { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = true)]
        public DateTime FirstFlightDate { get; set; }

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
        /// Gets or sets the next maintenance date.
        /// </summary>
        [BiaDtoField(Type = "date", Required = true)]
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        [BiaDtoField(Type = "time", Required = false)]
        public string SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour for flight data.
        /// </summary>
        [BiaDtoField(Type = "time", Required = true)]
        public string SyncFlightDataTime { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        [BiaDtoField(Required = true)]
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the motors count.
        /// </summary>
        [BiaDtoField(Required = false)]
        public int? MotorsCount { get; set; }

        /// <summary>
        /// Gets or sets the total of flight hours.
        /// </summary>
        [BiaDtoField(Required = true)]
        public double TotalFlightHours { get; set; }

        /// <summary>
        /// Gets or sets the probability.
        /// </summary>
        [BiaDtoField(Required = false)]
        public double? Probability { get; set; }

        /// <summary>
        /// Gets or sets the fuel capacity.
        /// </summary>
        [BiaDtoField(Required = true)]
        public float FuelCapacity { get; set; }

        /// <summary>
        /// Gets or sets the fuelLevel.
        /// </summary>
        [BiaDtoField(Required = false)]
        public float? FuelLevel { get; set; }

        /// <summary>
        /// Gets or sets the original price.
        /// </summary>
        [BiaDtoField(Required = true)]
        public decimal OriginalPrice { get; set; }

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
        [BiaDtoField(ItemType = "Airport", Required = true)]
        public ICollection<OptionDto> ConnectingAirports { get; set; }

        /// <summary>
        /// Gets or sets the plane type title.
        /// </summary>
        [BiaDtoField(ItemType = "PlaneType")]
        public OptionDto PlaneType { get; set; }

        /// <summary>
        /// Gets or sets the list of similar plane types.
        /// </summary>
        [BiaDtoField(ItemType = "PlaneType")]
        public ICollection<OptionDto> SimilarTypes { get; set; }

        /// <summary>
        /// Gets or sets the current airport title.
        /// </summary>
        [BiaDtoField(ItemType = "Airport", Required = true)]
        public OptionDto CurrentAirport { get; set; }
    }
}
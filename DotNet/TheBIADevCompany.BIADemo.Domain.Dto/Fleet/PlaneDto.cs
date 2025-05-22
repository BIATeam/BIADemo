// BIADemo only
// <copyright file="PlaneDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Fleet
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "Site")]
    public class PlaneDto : BaseDtoVersionedFixableArchivable<int>
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
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Plane is active.
        /// </summary>
        [BiaDtoField(Required = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Plane is maintenance.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool? IsMaintenance { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime")]
        public DateTime FirstFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the last flight date.
        /// </summary>
        [BiaDtoField(Required = false, Type = "datetime")]
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        [BiaDtoField(Required = false, Type = "date")]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the next maintenance date.
        /// </summary>
        [BiaDtoField(Required = true, Type = "date")]
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the sync time.
        /// </summary>
        [BiaDtoField(Required = false, Type = "time")]
        public string SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the sync flight data time.
        /// </summary>
        [BiaDtoField(Required = true, Type = "time")]
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
        /// Gets or sets the total flight hours.
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
        /// Gets or sets the fuel level.
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
        /// Gets or sets the plane type.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "PlaneType")]
        public OptionDto PlaneType { get; set; }

        /// <summary>
        /// Gets or sets the list of similar types.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "PlaneType")]
        public ICollection<OptionDto> SimilarTypes { get; set; }

        /// <summary>
        /// Gets or sets the current airport.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Airport")]
        public OptionDto CurrentAirport { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Airport")]
        public ICollection<OptionDto> ConnectingAirports { get; set; }
    }
}
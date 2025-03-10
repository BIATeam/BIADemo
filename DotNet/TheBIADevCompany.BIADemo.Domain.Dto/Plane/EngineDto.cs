// BIADemo only
// <copyright file="EngineDto.cs" company="TheBIADevCompany">
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
    /// The DTO used to represent a Engine.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "Site")]
    public class EngineDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer's name.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the next maintenance date.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = true)]
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the last maintenance date.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = false)]
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        [BiaDtoField(Type = "date", Required = true)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the exchange date.
        /// </summary>
        [BiaDtoField(Type = "date", Required = false)]
        public DateTime? ExchangeDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        [BiaDtoField(Type = "time", Required = true)]
        public string SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the daily ignition hour.
        /// </summary>
        [BiaDtoField(Type = "time", Required = false)]
        public string IgnitionTime { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        [BiaDtoField(Required = false)]
        public int? Power { get; set; }

        /// <summary>
        /// Gets or sets the noise level.
        /// </summary>
        [BiaDtoField(Required = true)]
        public int NoiseLevel { get; set; }

        /// <summary>
        /// Gets or sets the flight hours.
        /// </summary>
        [BiaDtoField(Required = true)]
        public double FlightHours { get; set; }

        /// <summary>
        /// Gets or sets the average of flight hours.
        /// </summary>
        [BiaDtoField(Required = false)]
        public double? AverageFlightHours { get; set; }

        /// <summary>
        /// Gets or sets the fuel consumption.
        /// </summary>
        [BiaDtoField(Required = true)]
        public float FuelConsumption { get; set; }

        /// <summary>
        /// Gets or sets the average of fuel consumption.
        /// </summary>
        [BiaDtoField(Required = false)]
        public float? AverageFuelConsumption { get; set; }

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
        /// Gets or sets a value indicating whether this instance is to be maintained.
        /// </summary>
        [BiaDtoField(Required = true)]
        public bool IsToBeMaintained { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hybrid.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool? IsHybrid { get; set; }

        /// <summary>
        /// Gets or sets the Plane.
        /// </summary>
        [BiaDtoField(IsParent = true, Required = true)]
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the principal part serial number.
        /// </summary>
        [BiaDtoField(ItemType = "Part")]
        public OptionDto PrincipalPart { get; set; }

        /// <summary>
        /// Gets or sets the list of installed part'ss serial numbers.
        /// </summary>
        [BiaDtoField(ItemType = "Part")]
        public ICollection<OptionDto> InstalledParts { get; set; }
    }
}
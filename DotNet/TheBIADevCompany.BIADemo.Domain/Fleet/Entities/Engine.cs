// BIADemo only
// <copyright file="Engine.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The Engine entity.
    /// </summary>
    public class Engine : VersionedTable, IEntityFixable<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer's name.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the next maintenance date.
        /// </summary>
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the last maintenance date.
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the exchange date.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? ExchangeDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the daily ignition hour.
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan? IgnitionTime { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        public int? Power { get; set; }

        /// <summary>
        /// Gets or sets the noise level.
        /// </summary>
        public int NoiseLevel { get; set; }

        /// <summary>
        /// Gets or sets the flight hours.
        /// </summary>
        public double FlightHours { get; set; }

        /// <summary>
        /// Gets or sets the average of flight hours.
        /// </summary>
        public double? AverageFlightHours { get; set; }

        /// <summary>
        /// Gets or sets the fuel consumption.
        /// </summary>
        public float FuelConsumption { get; set; }

        /// <summary>
        /// Gets or sets the average of fuel consumption.
        /// </summary>
        public float? AverageFuelConsumption { get; set; }

        /// <summary>
        /// Gets or sets the original price.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the estimated price.
        /// </summary>
        public decimal? EstimatedPrice { get; set; }

        /// <summary>
        /// Gets or sets the Plane.
        /// </summary>
        public virtual Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the Plane id.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is to be maintained.
        /// </summary>
        public bool IsToBeMaintained { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hybrid.
        /// </summary>
        public bool? IsHybrid { get; set; }

        /// <summary>
        /// Gets or sets the principal part.
        /// </summary>
        public Part PrincipalPart { get; set; }

        /// <summary>
        /// Gets or sets the principal part id.
        /// </summary>
        public int? PrincipalPartId { get; set; }

        /// <summary>
        /// Gets or sets the installed parts.
        /// </summary>
        public ICollection<Part> InstalledParts { get; set; }

        /// <summary>
        /// Gets or sets the installed parts. Via jointure table.
        /// </summary>
        public ICollection<EnginePart> InstalledEngineParts { get; set; }

        /// <inheritdoc/>
        public bool IsFixed { get; set; }

        /// <inheritdoc/>
        public DateTime? FixedDate { get; set; }
    }
}
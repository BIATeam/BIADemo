// BIADemo only
// <copyright file="Plane.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Audit.EntityFramework;
    using BIA.Net.Core.Domain.Entity;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The plane entity.
    /// </summary>
    [AuditInclude]
    public class Plane : BaseEntityVersionedFixableArchivable<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is on maintenance.
        /// </summary>
        public bool? IsMaintenance { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        public DateTime FirstFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the last flight date.
        /// </summary>
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the next maintenance date.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronization hour.
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan? SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronization hour for flight data.
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan SyncFlightDataTime { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the motors count.
        /// </summary>
        public int? MotorsCount { get; set; }

        /// <summary>
        /// Gets or sets the total of flight hours.
        /// </summary>
        public double TotalFlightHours { get; set; }

        /// <summary>
        /// Gets or sets the probability.
        /// </summary>
        public double? Probability { get; set; }

        /// <summary>
        /// Gets or sets the fuel capacity.
        /// </summary>
        public float FuelCapacity { get; set; }

        /// <summary>
        /// Gets or sets the fuelLevel.
        /// </summary>
        public float? FuelLevel { get; set; }

        /// <summary>
        /// Gets or sets the original price.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the estimated price.
        /// </summary>
        public decimal? EstimatedPrice { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual Site Site { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports. Direct access.
        /// </summary>
        public ICollection<Airport> ConnectingAirports { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports. Via the jointure table.
        /// </summary>
        public ICollection<PlaneAirport> ConnectingPlaneAirports { get; set; }

        /// <summary>
        /// Gets or sets the  plane type.
        /// </summary>
        public virtual PlaneType PlaneType { get; set; }

        /// <summary>
        /// Gets or sets the plane type id.
        /// </summary>
        public int? PlaneTypeId { get; set; }

        /// <summary>
        /// Gets or sets the current airport.
        /// </summary>
        public virtual Airport CurrentAirport { get; set; }

        /// <summary>
        /// Gets or sets the current airport id.
        /// </summary>
        public int CurrentAirportId { get; set; }

        /// <summary>
        /// Gets or sets the list of similar plane type. Direct access.
        /// </summary>
        public ICollection<PlaneType> SimilarPlaneTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of similar plane type. Via the jointure table.
        /// </summary>
        public ICollection<PlanePlaneType> SimilarPlanePlaneTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of engines for plane.
        /// </summary>
        public ICollection<Engine> Engines { get; set; }
    }
}
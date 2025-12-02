// <copyright file="PlaneSpecificQueryModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.QueryModels
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.QueryModel;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// Represents a data transfer object for querying plane-specific information from database.
    /// </summary>
    public class PlaneSpecificQueryModel : BaseQueryModel<int>, IEntityFixable, IEntityVersioned
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
        /// Gets or sets the last flight date.
        /// </summary>
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronization hour for flight data.
        /// </summary>
        public TimeSpan SyncFlightDataTime { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the original price.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Gets or sets the ICollection of connecting airports. Direct access.
        /// </summary>
        public ICollection<Airport> ConnectingAirports { get; set; }

        /// <summary>
        /// Gets or sets the  plane type.
        /// </summary>
        public PlaneType PlaneType { get; set; }

        /// <summary>
        /// Gets or sets the current airport.
        /// </summary>
        public Airport CurrentAirport { get; set; }

        /// <summary>
        /// Gets or sets the ICollection of engines for plane.
        /// </summary>
        public ICollection<Engine> Engines { get; set; }

        /// <inheritdoc/>
        public bool IsFixed { get; set; }

        /// <inheritdoc/>
        public DateTime? FixedDate { get; set; }

        /// <inheritdoc/>
        public byte[] RowVersion { get; set; }
    }
}

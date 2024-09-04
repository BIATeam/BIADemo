// BIADemo only
// <copyright file="Engine.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The Engine entity.
    /// </summary>
    public class Engine : VersionedTable, IEntity<int>
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
        /// Gets or sets the last maintenance date.
        /// </summary>
        public DateTime LastMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        public int? Power { get; set; }

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
    }
}
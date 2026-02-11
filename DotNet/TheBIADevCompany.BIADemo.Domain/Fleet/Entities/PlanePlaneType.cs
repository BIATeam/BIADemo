// BIADemo only
// <copyright file="PlanePlaneType.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using Audit.EntityFramework;
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity conformcertif repair site.
    /// </summary>
    [AuditInclude]
    public class PlanePlaneType : VersionedTable
    {
        /// <summary>
        /// Gets or sets the conformcertif.
        /// </summary>
        public Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the conformcertif id.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public PlaneType PlaneType { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int PlaneTypeId { get; set; }
    }
}

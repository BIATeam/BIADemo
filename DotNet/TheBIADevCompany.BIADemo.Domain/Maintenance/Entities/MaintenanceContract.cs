// BIADemo only
// <copyright file="MaintenanceContract.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Entities
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The entity to link maintenance teams and countries.
    /// </summary>
    public class MaintenanceContract : BaseEntityVersionedFixableArchivable<int>
    {
        /// <summary>
        /// Gets or sets the contract number.
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or sets the contract description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports. Via the jointure table.
        /// </summary>
        public ICollection<MaintenanceContractPlane> MaintenanceContractPlanes { get; set; }

        /// <summary>
        /// Gets or sets the list of planes subjects to that maintenance contract.
        /// </summary>
        public ICollection<Plane> Planes { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual Site Site { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or sets the AircraftMaintenanceCompany.
        /// </summary>
        public virtual AircraftMaintenanceCompany AircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the AircraftMaintenanceCompany id.
        /// </summary>
        public int? AircraftMaintenanceCompanyId { get; set; }
    }
}

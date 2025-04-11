// BIADemo only
// <copyright file="MaintenanceContract.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The entity to link maintenance teams and countries.
    /// </summary>
    public class MaintenanceContract : VersionedTable, IEntityArchivable<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

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

        /// <inheritdoc/>
        public bool IsArchived { get; set; }

        /// <inheritdoc/>
        public DateTime? ArchivedDate { get; set; }

        /// <inheritdoc/>
        public bool IsFixed { get; set; }

        /// <inheritdoc/>
        public DateTime? FixedDate { get; set; }
    }
}

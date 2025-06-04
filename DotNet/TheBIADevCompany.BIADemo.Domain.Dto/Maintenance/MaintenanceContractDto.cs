// BIADemo only
// <copyright file="MaintenanceContractDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Maintenance
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a MaintenanceContract.
    /// </summary>
    public class MaintenanceContractDto : BaseDtoVersioned<int>
    {
        /// <summary>
        /// Gets or sets the AircraftMaintenanceCompany Id.
        /// </summary>
        [BiaDtoField(Required = false, IsParent = true)]
        public int AircraftMaintenanceCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the ContractNumber.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Planes.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Plane")]
        public ICollection<OptionDto> Planes { get; set; }

        /// <summary>
        /// Gets or sets the Site Id.
        /// </summary>
        [BiaDtoField(Required = false, IsParent = true)]
        public int SiteId { get; set; }
    }
}

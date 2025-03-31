// BIADemo only
// <copyright file="MaintenanceContractDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a MaintenanceContract.
    /// </summary>
    public class MaintenanceContractDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the AircraftMaintenanceCompany.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "AircraftMaintenanceCompany")]
        public OptionDto AircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the ArchivedDate.
        /// </summary>
        [BiaDtoField(Required = false, Type = "datetime")]
        public DateTime? ArchivedDate { get; set; }

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
        /// Gets or sets the FixedDate.
        /// </summary>
        [BiaDtoField(Required = false, Type = "datetime")]
        public DateTime? FixedDate { get; set; }

        /// <summary>
        /// Gets or sets the IsArchived.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the Planes.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Plane")]
        public ICollection<OptionDto> Planes { get; set; }

        /// <summary>
        /// Gets or sets the Site.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Site")]
        public OptionDto Site { get; set; }
    }
}

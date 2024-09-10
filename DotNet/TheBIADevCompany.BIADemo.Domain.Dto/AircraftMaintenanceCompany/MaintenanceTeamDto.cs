// BIADemo only
// <copyright file="MaintenanceTeamDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The DTO used to represent a MaintenanceTeamDto.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "AircraftMaintenanceCompany")]
    public class MaintenanceTeamDto : TeamDto
    {
        /// <summary>
        /// Gets or sets the aircraft maintenance company.
        /// </summary>
        [BiaDtoField(IsParent = true, Required = true)]
        public int AircraftMaintenanceCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team active indicator.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
// BIADemo only
// <copyright file="MaintenanceTeamDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The DTO used to represent a MaintenanceTeamDto.
    /// </summary>
    public class MaintenanceTeamDto : TeamDto
    {
        /// <summary>
        /// Gets or sets the aircraft maintenance company.
        /// </summary>
        public int AircraftMaintenanceCompanyId { get; set; }
    }
}
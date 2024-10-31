// BIADemo only
// <copyright file="IMaintenanceTeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The interface defining the application service for MaintenanceTeam.
    /// </summary>
    public interface IMaintenanceTeamAppService : ICrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto>
    {
    }
}
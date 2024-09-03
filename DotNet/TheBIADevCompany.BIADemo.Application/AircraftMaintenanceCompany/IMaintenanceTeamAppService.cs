// BIADemo only
// <copyright file="IMaintenanceTeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Application.Service;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The interface defining the application service for MaintenanceTeam.
    /// </summary>
    public interface IMaintenanceTeamAppService : ICrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto>
    {
    }
}
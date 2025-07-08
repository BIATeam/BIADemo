// BIADemo only
// <copyright file="IMaintenanceTeamAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    /// <summary>
    /// The interface defining the application service for maintenanceTeam.
    /// </summary>
    public interface IMaintenanceTeamAppService : ICrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto>
    {
    }
}
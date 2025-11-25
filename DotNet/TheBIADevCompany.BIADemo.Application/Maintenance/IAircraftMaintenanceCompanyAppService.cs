// BIADemo only
// <copyright file="IAircraftMaintenanceCompanyAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    /// <summary>
    /// The interface defining the application service for aircraftMaintenanceCompany.
    /// </summary>
    public interface IAircraftMaintenanceCompanyAppService : ICrudAppServiceBase<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany, int, PagingFilterFormatDto<TeamAdvancedFilterDto>>
    {
    }
}
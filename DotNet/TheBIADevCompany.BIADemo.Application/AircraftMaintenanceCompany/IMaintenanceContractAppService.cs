// BIADemo only
// <copyright file="IMaintenanceContractAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The interface defining the application service for maintenanceContract.
    /// </summary>
    public interface IMaintenanceContractAppService : ICrudAppServiceBase<MaintenanceContractDto, MaintenanceContract, int, PagingFilterFormatDto>
    {
    }
}

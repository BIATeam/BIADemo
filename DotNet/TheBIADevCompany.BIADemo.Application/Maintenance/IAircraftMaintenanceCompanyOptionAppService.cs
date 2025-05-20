// BIADemo only
// <copyright file="IAircraftMaintenanceCompanyOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for aircraftMaintenanceCompany option.
    /// </summary>
    public interface IAircraftMaintenanceCompanyOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}

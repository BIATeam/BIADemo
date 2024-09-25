// BIADemo only
// <copyright file="ICountryOptionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The interface defining the application service for country option.
    /// </summary>
    public interface ICountryOptionAppService : IOptionAppServiceBase<CountryOptionDto, int>
    {
    }
}
// BIADemo only
// <copyright file="ICountryAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface ICountryAppService : ICrudAppServiceBase<CountryDto, Country, int, PagingFilterFormatDto>
    {
        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync();
    }
}
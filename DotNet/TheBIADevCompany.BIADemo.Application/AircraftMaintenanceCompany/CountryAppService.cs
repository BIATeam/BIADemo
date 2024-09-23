// BIADemo only
// <copyright file="CountryAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class CountryAppService : CrudAppServiceBase<CountryDto, Country, int, PagingFilterFormatDto, CountryMapper>, ICountryAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CountryAppService(ITGenericRepository<Country, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return GetAllAsync<OptionDto, CountryOptionMapper>();
        }
    }
}
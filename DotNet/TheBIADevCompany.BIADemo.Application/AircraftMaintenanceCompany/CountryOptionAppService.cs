// BIADemo only
// <copyright file="CountryOptionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for country option.
    /// </summary>
    public class CountryOptionAppService : OptionAppServiceBase<CountryOptionDto, Country, int, CountryOptionMapper>, ICountryOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CountryOptionAppService(ITGenericRepository<Country, int> repository)
            : base(repository)
        {
        }
    }
}
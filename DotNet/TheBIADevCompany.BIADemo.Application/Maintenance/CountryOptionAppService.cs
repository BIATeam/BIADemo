// BIADemo only
// <copyright file="CountryOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers;

    /// <summary>
    /// The application service used for country option.
    /// </summary>
    public class CountryOptionAppService : OptionAppServiceBase<OptionDto, Country, int, CountryOptionMapper>, ICountryOptionAppService
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
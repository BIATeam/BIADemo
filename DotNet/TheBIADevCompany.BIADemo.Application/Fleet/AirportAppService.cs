// BIADemo only
// <copyright file="AirportAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;

    /// <summary>
    /// The application service used for airport.
    /// </summary>
    public class AirportAppService : CrudAppServiceBase<AirportDto, Airport, int, PagingFilterFormatDto, AirportMapper>, IAirportAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AirportAppService(ITGenericRepository<Airport, int> repository)
            : base(repository)
        {
        }
    }
}
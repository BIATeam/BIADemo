// BIADemo only
// <copyright file="AirportAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Mappers;

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
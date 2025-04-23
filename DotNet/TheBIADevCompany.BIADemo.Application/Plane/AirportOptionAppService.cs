// BIADemo only
// <copyright file="AirportOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Mappers;

    /// <summary>
    /// The application service used for Airport option.
    /// </summary>
    public class AirportOptionAppService : OptionAppServiceBase<OptionDto, Airport, int, AirportOptionMapper>, IAirportOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AirportOptionAppService(ITGenericRepository<Airport, int> repository)
            : base(repository)
        {
        }
    }
}
// BIADemo only
// <copyright file="PlaneTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;

    /// <summary>
    /// The application service used for plane type option.
    /// </summary>
    public class PlaneTypeOptionAppService : OptionAppServiceBase<OptionDto, PlaneType, int, PlaneTypeOptionMapper>, IPlaneTypeOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTypeOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaneTypeOptionAppService(ITGenericRepository<PlaneType, int> repository)
            : base(repository)
        {
        }
    }
}
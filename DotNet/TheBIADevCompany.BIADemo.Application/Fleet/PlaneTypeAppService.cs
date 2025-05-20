// BIADemo only
// <copyright file="PlaneTypeAppService.cs" company="TheBIADevCompany">
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
    /// The application service used for plane.
    /// </summary>
    public class PlaneTypeAppService : CrudAppServiceBase<PlaneTypeDto, PlaneType, int, PagingFilterFormatDto, PlaneTypeMapper>, IPlaneTypeAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTypeAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaneTypeAppService(ITGenericRepository<PlaneType, int> repository)
            : base(repository)
        {
        }
    }
}
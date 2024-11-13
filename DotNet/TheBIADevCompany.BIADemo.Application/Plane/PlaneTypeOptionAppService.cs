// BIADemo only
// <copyright file="PlaneTypeOptionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The application service used for PlaneType option.
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
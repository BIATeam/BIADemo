// BIADemo only
// <copyright file="PlaneAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneAppService : CrudAppServiceBase<PlaneDto, Plane, LazyLoadDto, PlaneMapper>, IPlaneAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="planeQueryCustomizer">The plane query customizer for include.</param>
        public PlaneAppService(ITGenericRepository<Plane> repository, IPrincipal principal)
            : base(repository)
        {
            this.currentSiteId = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>().CurrentSiteId;
            this.filtersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentSiteId));
        }

        /// <inheritdoc/>
        public override Task<PlaneDto> AddAsync(PlaneDto dto, string mapperMode = null)
        {
            dto.Site = new Domain.Dto.Site.SiteDto { Id = this.currentSiteId };
            return base.AddAsync(dto, mapperMode);
        }

        /// <summary>
        /// Return a range to use in Calc SpreadSheet.
        /// </summary>
        /// <param name="filters">The filter.</param>
        /// <returns><see cref="Task"/>Representing the asynchronous operation.</returns>
        public async Task<(IEnumerable<PlaneDto> Results, int Total)> GetRangeForCalcAsync(LazyLoadDto filters = null)
        {
            return await this.GetRangeAsync<PlaneDto, PlaneMapper, LazyLoadDto>(filters: filters);
        }
    }
}
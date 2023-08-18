// <copyright file="SiteAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The application service used for site.
    /// </summary>
    public class SiteAppService : CrudAppServiceBase<SiteDto, Site, int, PagingFilterFormatDto<SiteAdvancedFilterDto>, SiteMapper>, ISiteAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public SiteAppService(ITGenericRepository<Site, int> repository, IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            int currentId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            IEnumerable<string> currentUserPermissions = (principal as BiaClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == Rights.Teams.AccessAll) == true;
            int userId = (principal as BiaClaimsPrincipal).GetUserId();

            this.FiltersContext.Add(
                AccessMode.Read,
                new DirectSpecification<Site>(p => accessAll || p.Members.Any(m => m.UserId == userId)));

            this.FiltersContext.Add(
                AccessMode.Update,
                new DirectSpecification<Site>(p => accessAll || p.Id == currentId));
        }

        /// <inheritdoc cref="ISiteAppService.GetRangeWithMembersAsync"/>
        public async Task<(IEnumerable<SiteInfoDto> Sites, int Total)> GetRangeWithMembersAsync(PagingFilterFormatDto<SiteAdvancedFilterDto> filters)
        {
            return await this.GetRangeAsync<SiteInfoDto, SiteInfoMapper, PagingFilterFormatDto<SiteAdvancedFilterDto>>(filters: filters, specification: SiteSpecification.SearchGetAll(filters));
        }

        /// <inheritdoc cref="ISiteAppService.GetWithMembersAsync"/>
        public async Task<SiteInfoDto> GetWithMembersAsync(int id)
        {
            return await this.Repository.GetResultAsync(this.InitMapper<SiteInfoDto, SiteInfoMapper>().EntityToDto(), id: id);
        }
    }
}
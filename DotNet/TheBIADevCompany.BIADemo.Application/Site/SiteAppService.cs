// <copyright file="SiteAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The application service used for site.
    /// </summary>
    public class SiteAppService : CrudAppServiceBase<SiteDto, Site, SiteFilterDto, SiteMapper>, ISiteAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public SiteAppService(ITGenericRepository<Site> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <inheritdoc cref="ISiteAppService.GetAllWithMembersAsync"/>
        public async Task<(IEnumerable<SiteInfoDto> Sites, int Total)> GetAllWithMembersAsync(SiteFilterDto filters)
        {
            UserDataDto userData = this.principal.GetUserData<UserDataDto>();
            IEnumerable<string> currentUserRights = this.principal.GetUserRights();
            int siteId = currentUserRights?.Any(x => x == Rights.Sites.AccessAll) == true ? default(int) : userData.CurrentSiteId;

            return await this.GetRangeAsync<SiteInfoDto, SiteInfoMapper, SiteFilterDto>(filters: filters, specification: SiteSpecification.SearchGetAll(filters, siteId));
        }

        /// <inheritdoc cref="ISiteAppService.GetAllAsync"/>
        public async Task<IEnumerable<SiteDto>> GetAllAsync(int userId = 0, IEnumerable<string> userRights = null)
        {
            userRights = userRights != null ? userRights : this.principal.GetUserRights();
            userId = userId > 0 ? userId : this.principal.GetUserId();

            if (userRights?.Any(x => x == Rights.Sites.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(new SiteMapper().EntityToDto(userId));
            }
            else
            {
                return await this.Repository.GetAllResultAsync(new SiteMapper().EntityToDto(userId), specification: new DirectSpecification<Site>(site => site.Members.Any(member => member.UserId == userId)));
            }
        }
    }
}
// <copyright file="RoleAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for role.
    /// </summary>
    public class RoleAppService : FilteredServiceBase<Role>, IRoleAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        public RoleAppService(ITGenericRepository<Role> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, RoleOptionMapper>();
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <param name="siteId">The site Id.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>List of OptionDto.</returns>
        public async Task<IEnumerable<OptionDefaultDto>> GetMemberRolesAsync(int siteId, int userId)
        {
            return await this.Repository.GetAllResultAsync<OptionDefaultDto>(
                role => new OptionDefaultDto
                {
                    Id = role.Id,
                    Display = role.Code,
                    IsDefault = role.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.IsDefault),
                },
                filter: x => x.MemberRoles.Select(mr => mr.Member).Any(m => m.SiteId == siteId && m.UserId == userId));
        }
    }
}
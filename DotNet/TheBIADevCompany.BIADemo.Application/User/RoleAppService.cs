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
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
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

        /// <inheritdoc cref="IRoleAppService.GetAllAsync"/>
        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var userId = this.principal.GetUserId();
            return await this.Repository.GetAllResultAsync<RoleDto>(role => new RoleDto
            {
                Id = role.Id,
                LabelEn = role.LabelEn,
                LabelFr = role.LabelFr,
                LabelEs = role.LabelEs,
                IsDefault = role.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.IsDefault),
            });
        }

        /// <inheritdoc cref="IRoleAppService.GetMemberRolesAsync"/>
        public async Task<IEnumerable<RoleDto>> GetMemberRolesAsync(int siteId, int userId)
        {
            return await this.Repository.GetAllResultAsync<RoleDto>(
                role => new RoleDto
                {
                    Id = role.Id,
                    LabelEn = role.LabelEn,
                    LabelFr = role.LabelFr,
                    LabelEs = role.LabelEs,
                    IsDefault = role.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.IsDefault),
                },
                filter: x => x.MemberRoles.Select(mr => mr.Member).Any(m => m.SiteId == siteId && m.UserId == userId));
        }
    }
}
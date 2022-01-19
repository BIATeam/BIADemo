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
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for role.
    /// </summary>
    public class RoleAppService : FilteredServiceBase<Role, int>, IRoleAppService
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
        /// <param name="userContext">The user context.</param>
        public RoleAppService(ITGenericRepository<Role, int> repository, IPrincipal principal, UserContext userContext)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
            this.userContext = userContext;
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
        public async Task<IEnumerable<RoleDto>> GetMemberRolesAsync(int siteId, int userId)
        {
            return await this.Repository.GetAllResultAsync<RoleDto>(
                entity => new RoleDto
                {
                    Id = entity.Id,
                    Label = entity.Label,
                    Code = entity.Code,
                    IsDefault = entity.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.IsDefault),

                    // Mapping relationship *-1 : ICollection<Airports>
                    RoleTranslations = entity.RoleTranslations.Select(rt => new RoleTranslationDto
                    {
                        Id = rt.Id,
                        LanguageId = rt.LanguageId,
                        Label = rt.Label,
                        DtoState = DtoState.Unchanged,
                    }).ToList(),
                },
                filter: x => x.MemberRoles.Select(mr => mr.Member).Any(m => m.SiteId == siteId && m.UserId == userId));
        }
    }
}
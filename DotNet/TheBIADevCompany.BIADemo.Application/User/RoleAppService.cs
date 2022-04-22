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
        /// Initializes a new instance of the <see cref="RoleAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userContext">The user context.</param>
        public RoleAppService(ITGenericRepository<Role, int> repository, UserContext userContext)
            : base(repository)
        {
            this.userContext = userContext;
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        /// <param name="teamTypeId">The team type id.</param>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync(int teamTypeId)
        {
            return this.GetAllAsync<OptionDto, RoleOptionMapper>(filter: r => r.TeamTypes.Any(t => t.Id == teamTypeId));
        }

        /// <summary>
        /// Return the list of role of a user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>List of role code.</returns>
        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            return await this.Repository.GetAllResultAsync<string>(
                entity => entity.Code,
                filter: x => x.Users.Any(m => m.Id == userId));
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>List of OptionDto.</returns>
        public async Task<IEnumerable<RoleDto>> GetMemberRolesAsync(int teamId, int userId)
        {
            return await this.Repository.GetAllResultAsync<RoleDto>(
                entity => new RoleDto
                {
                    Id = entity.Id,
                    Display = "TODO: Remove this function",
                    Code = entity.Code,
                    IsDefault = entity.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.Member.TeamId == teamId && mr.IsDefault),
                },
                filter: x => x.MemberRoles.Select(mr => mr.Member).Any(m => m.TeamId == teamId && m.UserId == userId));
        }
    }
}
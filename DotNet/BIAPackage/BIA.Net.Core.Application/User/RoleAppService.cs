// <copyright file="RoleAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Mappers;

    /// <summary>
    /// The application service used for role.
    /// </summary>
    public class RoleAppService : DomainServiceBase<Role, int>, IRoleAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userContext">The user context.</param>
        public RoleAppService(ITGenericRepository<Role, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return the list of role of a user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>List of role code.</returns>
        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            return await this.Repository.GetAllResultAsync(
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
            return await this.Repository.GetAllResultAsync(
                entity => new RoleDto
                {
                    Id = entity.Id,
                    Display = "TODO: Remove this function",
                    Code = entity.Code,
                    IsDefault = entity.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.Member.TeamId == teamId && mr.IsDefault),
                },
                filter: x => x.MemberRoles.Select(mr => mr.Member).Any(m => m.TeamId == teamId && m.UserId == userId));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RoleDto>> GetAllTeamRolesAsync(int teamTypeId)
        {
            return await this.Repository.GetAllResultAsync(
                entity => new RoleDto
                {
                    Id = entity.Id,
                    Code = entity.Code,
                },
                filter: x => x.TeamTypes.Any(tt => tt.Id == teamTypeId));
        }
    }
}
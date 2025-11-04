// <copyright file="IMemberAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The interface defining the application service for member.
    /// </summary>
    public interface IMemberAppService : ICrudAppServiceBase<MemberDto, Member, int, PagingFilterFormatDto>
    {
        /// <summary>
        /// Get the list of MemberDto with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of MemberDto.</returns>
        Task<(IEnumerable<MemberDto> Members, int Total)> GetRangeByTeamAsync(PagingFilterFormatDto filters);

        /// <summary>
        /// Add several members or add only right if user already in list.
        /// </summary>
        /// <param name="membersDto">The members DTO.</param>
        /// <param name="addFromRoleApi">Indicates if the roles to add to the user comes from an external API.</param>
        /// <returns>The result of the creation.</returns>
        Task<IEnumerable<MemberDto>> AddUsers(MembersDto membersDto, bool addFromRoleApi = false);

        /// <summary>
        /// Removes a member and is roles.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="teamId">The id of the team.</param>
        /// <param name="removeManualRoles">Indicates whether to remove only automatic roles (flag isFromRoleApi) when false or all roles when true. Default value false.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveRolesAndUserFromTeam(int userId, int teamId, bool removeManualRoles = false);

        /// <summary>
        /// Sets the default role.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="roleIds">The roles identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultRoleAsync(int teamId, List<int> roleIds);

        /// <summary>
        /// Resets the default role.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ResetDefaultRoleAsync(int teamId);

        /// <summary>
        /// Generates CSV content.
        /// </summary>
        /// <param name="filters">Represents the columns and their translations.</param>
        /// <returns>A <see cref="Task"/> holding the buffered data to return in a file.</returns>
        Task<byte[]> GetCsvAsync(PagingFilterFormatDto filters);
    }
}
// <copyright file="IMemberAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;

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
        /// <returns>The result of the creation.</returns>
        Task<IEnumerable<MemberDto>> AddUsers(MembersDto membersDto);

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
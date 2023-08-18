// <copyright file="IRoleAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for role.
    /// </summary>
    public interface IRoleAppService : ICrudAppServiceBase<RoleDto, Role, int, PagingFilterFormatDto>
    {
        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <param name="teamTypeId">The team type id.</param>
        /// <returns>The list of production sites.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync(int teamTypeId);

        /// <summary>
        /// Return the list of role of a user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>List of role code.</returns>
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);

        /// <summary>
        /// Get all member roles.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The list of roles for this member.</returns>
        Task<IEnumerable<RoleDto>> GetMemberRolesAsync(int teamId, int userId);
    }
}
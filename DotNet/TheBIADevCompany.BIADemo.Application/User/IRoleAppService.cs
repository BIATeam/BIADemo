// <copyright file="IRoleAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for role.
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// Get all existing roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        Task<IEnumerable<RoleDto>> GetAllAsync();

        /// <summary>
        /// Get all member roles.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The list of roles for this member.</returns>
        Task<IEnumerable<RoleDto>> GetMemberRolesAsync(int siteId, int userId);
    }
}
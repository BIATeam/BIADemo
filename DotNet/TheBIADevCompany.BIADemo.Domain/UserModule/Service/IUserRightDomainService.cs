// <copyright file="IUserRightDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the user right domain service.
    /// </summary>
    public interface IUserRightDomainService
    {
        /// <summary>
        /// Get all rights for a user login.
        /// </summary>
        /// <param name="userDirectoryRoles">The user directory role.</param>
        /// <param name="sid">The user sid.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>
        /// The DTO containing the right list.
        /// </returns>
        Task<List<string>> GetRightsForUserAsync(List<string> userDirectoryRoles, string sid, int siteId = 0, int roleId = 0);

        /// <summary>
        /// Translate the roles in rights.
        /// </summary>
        /// <param name="roles">The liste of roles.</param>
        /// <returns>The list of rights.</returns>
        List<string> TranslateRolesInRights(List<string> roles);
    }
}
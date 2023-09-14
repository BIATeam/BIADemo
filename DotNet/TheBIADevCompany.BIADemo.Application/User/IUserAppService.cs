// <copyright file="IUserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for user.
    /// </summary>
    public interface IUserAppService
    {
        /// <summary>
        /// Get all existing users filtered.
        /// </summary>
        /// <param name="filter">Used to filter the users.</param>
        /// <returns>The list of users found.</returns>
        Task<IEnumerable<UserDto>> GetAllAsync(string filter);

        /// <summary>
        /// Get all existing users filtered.
        /// </summary>
        /// <param name="filters">Used to filter the users.</param>
        /// <returns>The list of users found and the total number of user.</returns>
        Task<(IEnumerable<UserDto> Users, int Total)> GetAllAsync(LazyLoadDto filters);

        /// <summary>
        /// Gets user info with its sid and create if not exist.
        /// </summary>
        /// <param name="sid">The sid to search in ldap with.</param>
        /// <param name="login">The login to check in ldap.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> CreateUserInfoFromLdapAsync(string sid, string login);

        /// <summary>
        /// Gets user info with its login.
        /// </summary>
        /// <param name="login">The login to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetUserInfoAsync(string login);

        /// <summary>
        /// Get all rights for a user with its sid.
        /// </summary>
        /// <param name="userDirectoryRoles">The user roles in user directory.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>
        /// The list of right.
        /// </returns>
        Task<List<string>> GetRightsForUserAsync(List<string> userDirectoryRoles, int userId, int siteId = 0, int roleId = 0);

        /// <summary>
        /// Translate the roles in rights.
        /// </summary>
        /// <param name="roles">List of roles.</param>
        /// <returns>Liste of rights.</returns>
        List<string> TranslateRolesInRights(List<string> roles);

        /// <summary>
        /// Get all roles for a user with its sid.
        /// </summary>
        /// <param name="isUserInDB">true if user is in database.</param>
        /// <param name="sid">The user sid.</param>
        /// <param name="domain">The user domain.</param>
        /// <returns>The list of roles.</returns>
        Task<List<string>> GetUserDirectoryRolesAsync(bool isUserInDB, string sid, string domain);

        /// <summary>
        /// Gets the profile of the given user.
        /// </summary>
        /// <param name="login">The user login.</param>
        /// <returns>The user profile.</returns>
        Task<UserProfileDto> GetUserProfileAsync(string login);

        /// <summary>
        /// Gets all AD user corresponding to a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="ldapName">The name of the LDAP domain to search in.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="users">The list of users to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> AddFromDirectory(IEnumerable<UserFromDirectoryDto> users);

        /// <summary>
        /// Remove a user in a group in AD.
        /// </summary>
        /// <param name="id">The identifier of the user to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> RemoveInGroupAsync(int id);

        /// <summary>
        /// Synchronize the users with the AD.
        /// </summary>
        /// <param name="fullSynchro">If true resynchronize existing user.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SynchronizeWithADAsync(bool fullSynchro = false);

        /// <summary>
        /// Updates the last login date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="activate">activate the user.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateLastLoginDateAndActivate(int userId, bool activate);

        /// <summary>
        /// Selects the default language.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        void SelectDefaultLanguage(UserInfoDto userInfo);

        /// <summary>
        /// Return all domain with conatinning users.
        /// </summary>
        /// <returns>List of dommain keys.</returns>
        Task<List<string>> GetAllLdapUsersDomains();
    }
}
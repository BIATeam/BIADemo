// <copyright file="IUserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for user.
    /// </summary>
    public interface IUserAppService : IFilteredServiceBase<User, int>
    {
        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <param name="filter">Used to filter the users.</param>
        /// /// <returns>The list of production sites.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync(string filter = null);

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
        /// Gets user info with its guid.
        /// </summary>
        /// <param name="guid">The guid to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetUserInfoAsync(Guid guid);

        /// <summary>
        /// Get all roles for a user with its sid.
        /// </summary>
        /// <param name="isUserInDB">true if user is in database.</param>
        /// <param name="sid">The user sid.</param>
        /// <param name="domain">The user domain.</param>
        /// <returns>The list of roles.</returns>
        Task<List<string>> GetUserDirectoryRolesAsync(bool isUserInDB, string sid, string domain);

        /// <summary>
        /// Gets all AD user corresponding to a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="ldapName">The name of the LDAP domain to search in.</param>
        /// <param name="max">The max number of items to return.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null, int max = 10);

        /// <summary>
        /// Gets all IdP user corresponding to a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="max">The max number of items to return.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserFromDirectoryDto>> GetAllIdpUserAsync(string filter, int max = 10);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="users">The list of users to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ResultAddUsersFromDirectoryDto> AddFromDirectory(IEnumerable<UserFromDirectoryDto> users);

        /// <summary>
        /// Adds from identity provider identifier.
        /// </summary>
        /// <param name="userFromDirectoryDtos">The user from directory dtos.</param>
        /// <returns>A <see cref="ResultAddUsersFromDirectoryDto"/>.</returns>
        Task<ResultAddUsersFromDirectoryDto> AddFromIdPAsync(IEnumerable<UserFromDirectoryDto> userFromDirectoryDtos);

        /// <summary>
        /// Deactivates the users asynchronous.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeactivateUsersAsync(List<int> ids);

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
        /// Return all domain with conatinning users.
        /// </summary>
        /// <returns>List of dommain keys.</returns>
        Task<List<string>> GetAllLdapUsersDomains();

        /// <summary>
        /// Selects the default language.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        void SelectDefaultLanguage(UserInfoDto userInfo);

        /// <summary>
        /// Get Csv.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>binary csv.</returns>
        Task<byte[]> GetCsvAsync(PagingFilterFormatDto filters);
    }
}
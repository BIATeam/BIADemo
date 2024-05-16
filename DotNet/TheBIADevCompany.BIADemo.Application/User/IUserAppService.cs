// <copyright file="IUserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
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
        /// Adds the user in DB from UserFromDirectory.
        /// </summary>
        /// <param name="identityKey">The identity key.</param>
        /// <param name="userFromDirectory">The user from directory.</param>
        /// <returns>The user in DB.</returns>
        Task<User> AddUserFromUserDirectoryAsync(string identityKey, UserFromDirectory userFromDirectory);

        /// <summary>
        /// Creates the user information from user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A UserInfoDto.</returns>
        UserInfoDto CreateUserInfo(User user);

        /// <summary>
        /// Gets user info with its login.
        /// </summary>
        /// <param name="identityKey">The identityKey to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetUserInfoAsync(string identityKey);

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
        /// <param name="first">Index start.</param>
        /// <param name="max">The max number of items to return.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserFromDirectoryDto>> GetAllIdpUserAsync(string filter, int first = 0, int max = 10);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="users">The list of users to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ResultAddUsersFromDirectoryDto> AddFromDirectory(IEnumerable<UserFromDirectoryDto> users);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="user">The list of users to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ResultAddUsersFromDirectoryDto> AddByIdentityKeyAsync(UserDto user);

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
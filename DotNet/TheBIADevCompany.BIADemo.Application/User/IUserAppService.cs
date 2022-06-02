// <copyright file="IUserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
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
        /// Get all roles for a user with its sid.
        /// </summary>
        /// <param name="sid">The user sid.</param>
        /// <returns>The list of roles.</returns>
        Task<List<string>> GetUserDirectoryRolesAsync(string sid);

        /// <summary>
        /// Gets user info with its sid and create if not exist.
        /// </summary>
        /// <param name="sid">The sid to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetCreateUserInfoAsync(string sid);

        /// <summary>
        /// Gets user info with its sid.
        /// </summary>
        /// <param name="sid">The sid to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetUserInfoAsync(string sid);

        /// <summary>
        /// Gets user info with its guid.
        /// </summary>
        /// <param name="guid">The guid to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetUserInfoAsync(Guid guid);

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
        /// <param name="max">The max number of items to return.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null, int max = 10);

        /// <summary>
        /// Gets all IdP user corresponding to a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="max">The max number of items to return.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserFromDirectoryDto>> GetAllIdPUserAsync(string filter, int max = 10);

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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateLastLoginDateAndActivate(int userId);

        /// <summary>
        /// Return all domain with conatinning users.
        /// </summary>
        /// <returns>List of dommain keys.</returns>
        Task<List<string>> GetAllLdapUsersDomains();

        /// <summary>
        /// Adds an UserAD in the in database.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddInDBAsync(IEnumerable<UserFromDirectoryDto> users);

        /// <summary>
        /// Generates CSV content.
        /// </summary>
        /// <param name="filters">Represents the columns and their traductions.</param>
        /// <returns>A <see cref="Task"/> holding the buffered data to return in a file.</returns>
        Task<byte[]> ExportCSV(PagingFilterFormatDto filters);

        /// <summary>
        /// Selects the default language.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        void SelectDefaultLanguage(UserInfoDto userInfo);
    }
}
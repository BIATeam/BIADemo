// <copyright file="IBaseUserAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Models;

    /// <summary>
    /// The interface defining the application service for user.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserFromDirectoryDto">The type of user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    public interface IBaseUserAppService<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory> : ICrudAppServiceBase<TUserDto, TUser, int, PagingFilterFormatDto>
        where TUserDto : BaseUserDto, new()
        where TUser : BaseUser, IEntity<int>, new()
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : IUserFromDirectory, new()
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
        Task<TUser> AddUserFromUserDirectoryAsync(string identityKey, TUserFromDirectory userFromDirectory);

        /// <summary>
        /// Creates the user information from user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A UserInfoDto.</returns>
        UserInfoDto CreateUserInfo(TUser user);

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
        Task<IEnumerable<TUserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null, int max = 10);

        /// <summary>
        /// Gets all IdP user corresponding to a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="first">Index start.</param>
        /// <param name="max">The max number of items to return.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<TUserFromDirectoryDto>> GetAllIdpUserAsync(string filter, int first = 0, int max = 10);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="users">The list of users to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ResultAddUsersFromDirectoryDto> AddFromDirectory(IEnumerable<TUserFromDirectoryDto> users);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="userDto">The list of users to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ResultAddUsersFromDirectoryDto> AddByIdentityKeyAsync(TUserDto userDto);

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
        /// Synchronize users with Idp.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SynchronizeWithIdpAsync();

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
        /// Saves list of users.
        /// </summary>
        /// <param name="userDtos">List of users dto.</param>
        /// <returns>Error message.</returns>
        Task<string> SaveAsync(List<TUserDto> userDtos);

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

        /// <summary>
        /// Sets the default site.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="teamTypeId">The team type identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultTeamAsync(int teamId, int teamTypeId);

        /// <summary>
        /// Reset the default site.
        /// </summary>
        /// <param name="teamTypeId">The team type identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ResetDefaultTeamAsync(int teamTypeId);
    }
}
// <copyright file="UserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    public class UserAppService : FilteredServiceBase<User, int>, IUserAppService
    {
        /// <summary>
        /// The user synchronize domain service.
        /// </summary>
        private readonly IUserSynchronizeDomainService userSynchronizeDomainService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<UserAppService> logger;

        private readonly IIdentityProviderRepository identityProviderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAppService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userSynchronizeDomainService">The user synchronize domain service.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="userContext">The user context.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        public UserAppService(
            ITGenericRepository<User, int> repository,
            IUserSynchronizeDomainService userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ILogger<UserAppService> logger,
            UserContext userContext,
            IIdentityProviderRepository identityProviderRepository)
            : base(repository)
        {
            this.userSynchronizeDomainService = userSynchronizeDomainService;
            this.configuration = configuration.Value;
            this.userDirectoryHelper = userDirectoryHelper;
            this.logger = logger;
            this.userContext = userContext;
            this.identityProviderRepository = identityProviderRepository;

            this.filtersContext.Add(AccessMode.Read, new DirectSpecification<User>(u => u.IsActive));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync(string filter = null)
        {
            Specification<User> specification = null;
            if (!string.IsNullOrEmpty(filter))
            {
                specification = UserSpecification.Search(filter);
            }

            return this.GetAllAsync<OptionDto, UserOptionMapper>(specification: specification, queryOrder: new QueryOrder<User>().OrderBy(o => o.LastName).ThenBy(o => o.FirstName));
        }

        /// <inheritdoc cref="IUserPermissionDomainService.GetPermissionsForUserAsync"/>
        public async Task<List<string>> GetUserDirectoryRolesAsync(string sid)
        {
            return await this.userDirectoryHelper.GetUserRolesBySid(sid);
        }

        /// <inheritdoc cref="IUserAppService.GetCreateUserInfoAsync"/>
        public async Task<UserInfoDto> GetCreateUserInfoAsync(string sid)
        {
            UserInfoDto userInfo = await this.GetUserInfoAsync(sid);

            if (userInfo != null)
            {
                this.SelectDefaultLanguage(userInfo);

                return userInfo;
            }

            // if user is not found in DB, try to synchronize from AD.
            UserFromDirectory userAD = await this.userDirectoryHelper.ResolveUserBySid(sid);

            if (userAD != null)
            {
                User user = new User();
                UserFromDirectory.UpdateUserFieldFromDirectory(user, userAD);

                this.Repository.Add(user);
                await this.Repository.UnitOfWork.CommitAsync();

                userInfo = new UserInfoDto
                {
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Country = user.Country,
                };
                this.SelectDefaultLanguage(userInfo);
            }

            return userInfo;
        }

        /// <inheritdoc cref="IUserAppService.GetUserInfoAsync"/>
        public async Task<UserInfoDto> GetUserInfoAsync(string sid)
        {
            return await this.Repository.GetResultAsync(UserSelectBuilder.SelectUserInfo(), filter: user => user.Sid == sid);
        }

        /// <inheritdoc cref="IUserAppService.GetUserInfoAsync"/>
        public async Task<UserInfoDto> GetUserInfoAsync(Guid guid)
        {
            return await this.Repository.GetResultAsync(UserSelectBuilder.SelectUserInfo(), filter: user => user.Guid == guid);
        }

        /// <inheritdoc cref="IUserAppService.GetUserProfileAsync"/>
        public async Task<UserProfileDto> GetUserProfileAsync(string login)
        {
            var profile = new UserProfileDto();

            var url = this.configuration.UserProfile.Url;
            if (!string.IsNullOrEmpty(url))
            {
                var parameters = new Dictionary<string, string> { { "login", login } };
                Dictionary<string, string> result;

                try
                {
                    result = await RequestHelper.GetAsync<Dictionary<string, string>>(url, parameters);
                }
                catch (Exception exception)
                {
                    result = new Dictionary<string, string>();
                    this.logger.LogError(exception, "An error occured while getting the user profile.");
                }

                foreach (var item in result)
                {
                    typeof(UserProfileDto).GetProperty(item.Key)?.SetValue(profile, item.Value);
                }
            }

            return profile;
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetAllLdapUsersDomains()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this.configuration.Authentication.LdapDomains.Where(d => d.ContainsUser).Select(d => d.Name).ToList();
        }

        /// <inheritdoc cref="IUserAppService.GetAllADUserAsync"/>
        public async Task<IEnumerable<UserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null, int max = 10)
        {
            return await Task.FromResult(this.userDirectoryHelper.SearchUsers(filter, ldapName, max).OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                .Select(UserFromDirectoryMapper.EntityToDto())
                .ToList());
        }

        /// <inheritdoc cref="IUserAppService.GetAllIdPUserAsync"/>
        public async Task<IEnumerable<UserFromDirectoryDto>> GetAllIdPUserAsync(string filter, int max = 10)
        {
            List<UserFromDirectory> userFromDirectories = await this.identityProviderRepository.SearchAsync(filter, max);
            return userFromDirectories.Select(UserFromDirectoryMapper.EntityToDto());
        }

        /// <inheritdoc cref="IUserAppService.AddFromDirectory"/>
        public async Task<ResultAddUsersFromDirectoryDto> AddFromDirectory(IEnumerable<UserFromDirectoryDto> users)
        {
            ResultAddUsersFromDirectoryDto result = new ResultAddUsersFromDirectoryDto();
            result.UsersAddedDtos = new List<OptionDto>();
            result.Errors = new List<string>();
            var ldapGroups = this.userDirectoryHelper.GetLdapGroupsForRole("User");
            result.Errors = new List<string>();
            if (ldapGroups != null && ldapGroups.Count > 0)
            {
                result.Errors = await this.userDirectoryHelper.AddUsersInGroup(users.Select(UserFromDirectoryMapper.DtoToEntity()).ToList(), "User");
                try
                {
                    await this.SynchronizeWithADAsync();
                    List<string> usersSid = users.Select(u => u.Sid).ToList();
                    result.UsersAddedDtos = (await this.Repository.GetAllEntityAsync(filter: x => usersSid.Contains(x.Sid))).Select(entity => new OptionDto
                    {
                        Id = entity.Id,
                        Display = entity.FirstName + " " + entity.LastName + " (" + entity.Login + ")",
                    }).ToList();
                }
                catch (Exception)
                {
                    result.Errors.Add("Error during synchronize. Retry Synchronize.");
                }
            }
            else
            {
                List<User> usersAdded = new List<User>();
                foreach (var userFormDirectoryDto in users)
                {
                    try
                    {
                        var foundUser = (await this.Repository.GetAllEntityAsync(filter: x => x.Sid == userFormDirectoryDto.Sid)).FirstOrDefault();

                        var addedUser = await this.userSynchronizeDomainService.AddOrActiveUserFromDirectory(userFormDirectoryDto.Sid, foundUser);

                        if (addedUser != null)
                        {
                            usersAdded.Add(addedUser);
                        }

                        await this.Repository.UnitOfWork.CommitAsync();
                    }
                    catch (Exception)
                    {
                        result.Errors.Add(userFormDirectoryDto.Domain + "\\" + userFormDirectoryDto.Login);
                    }
                }

                result.UsersAddedDtos = usersAdded.Select(entity => new OptionDto
                {
                    Id = entity.Id,
                    Display = entity.FirstName + " " + entity.LastName + " (" + entity.Login + ")",
                }).ToList();
            }

            return result;
        }

        /// <inheritdoc cref="IUserAppService.AddFromIdPAsync"/>
        public async Task<ResultAddUsersFromDirectoryDto> AddFromIdPAsync(IEnumerable<UserFromDirectoryDto> userFromDirectoryDtos)
        {
            ResultAddUsersFromDirectoryDto result = null;

            if (userFromDirectoryDtos?.Any() == true)
            {
                List<int> userIdAddeds = new List<int>();
                List<User> userToAdds = new List<User>();

                List<Guid> guids = userFromDirectoryDtos.Select(x => x.Guid).Where(x => x != Guid.Empty).ToList();
                List<User> userDbs = (await this.Repository.GetAllEntityAsync(filter: x => guids.Contains(x.Guid))).ToList();
                List<UserFromDirectoryDto> userFromDirectoryDtoToAdds = userFromDirectoryDtos.Where(x => !userDbs.Select(userDb => userDb.Guid).Contains(x.Guid)).ToList();

                // ADD
                List<UserFromDirectory> userFromDirectoryToAdds = PropertyMapper.Map<UserFromDirectoryDto, UserFromDirectory>(userFromDirectoryDtoToAdds).ToList();

                if (userFromDirectoryToAdds?.Any() == true)
                {
                    foreach (UserFromDirectory userFromDirectoryToAdd in userFromDirectoryToAdds)
                    {
                        User user = new User();
                        UserFromDirectory.UpdateUserFieldFromDirectory(user, userFromDirectoryToAdd);
                        user.Login = user.Login?.ToUpper();
                        if (string.IsNullOrWhiteSpace(user.Sid) && user.Guid != Guid.Empty)
                        {
                            user.Sid = user.Guid.ToString();
                        }

                        userToAdds.Add(user);
                    }

                    this.Repository.AddRange(userToAdds);
                }

                // UPDATE IsActive property
                List<User> userToUpdates = userDbs.Where(x => !x.IsActive).ToList();
                if (userToUpdates?.Any() == true)
                {
                    foreach (User userToUpdate in userToUpdates)
                    {
                        userToUpdate.IsActive = true;
                        this.Repository.SetModified(userToUpdate);
                    }
                }

                // SAVE
                await this.Repository.UnitOfWork.CommitAsync();

                // Fill userIdAddeds
                if (userToUpdates?.Any() == true)
                {
                    userIdAddeds.AddRange(userToUpdates.Select(x => x.Id).ToList());
                }

                if (userToAdds?.Any() == true)
                {
                    userIdAddeds.AddRange(userToAdds.Select(x => x.Id).ToList());
                }

                // Fill result object
                result = new ResultAddUsersFromDirectoryDto();
                result.UsersAddedDtos = (await this.GetAllAsync<OptionDto, UserOptionMapper>(
                    filter: x => userIdAddeds.Contains(x.Id),
                    queryOrder: new QueryOrder<User>().OrderBy(o => o.LastName).ThenBy(o => o.FirstName))).ToList();
            }

            return result;
        }

        /// <summary>
        /// Deactivates the users asynchronous.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeactivateUsersAsync(List<int> ids)
        {
            if (ids?.Any() == true)
            {
                List<User> users = (await this.Repository.GetAllEntityAsync(filter: x => x.IsActive && ids.Contains(x.Id))).ToList();

                if (users?.Any() == true)
                {
                    foreach (User user in users)
                    {
                        user.IsActive = false;
                        this.Repository.SetModified(user);
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }

        /// <inheritdoc cref="IUserAppService.RemoveInGroupAsync"/>
        public async Task<string> RemoveInGroupAsync(int id)
        {
            var ldapGroups = this.userDirectoryHelper.GetLdapGroupsForRole("User");
            var user = await this.Repository.GetEntityAsync(id: id);
            if (ldapGroups != null && ldapGroups.Count > 0)
            {
                if (user == null)
                {
                    return "User not found in database";
                }

                List<IUserFromDirectory> notRemovedUser = await this.userDirectoryHelper.RemoveUsersInGroup(new List<IUserFromDirectory>() { new UserFromDirectory() { Guid = user.Guid, Login = user.Login } }, "User");

                try
                {
                    await this.SynchronizeWithADAsync();
                }
                catch (Exception)
                {
                    return "Error during synchronize. Retry Synchronize.";
                }

                if (notRemovedUser.Count != 0)
                {
                    return "Not able to remove user. (Probably define in sub group)";
                }
            }
            else
            {
                this.userSynchronizeDomainService.DeactivateUser(user);
                await this.Repository.UnitOfWork.CommitAsync();
            }

            return string.Empty;
        }

        /// <inheritdoc cref="IUserAppService.SynchronizeWithADAsync"/>
        public async Task SynchronizeWithADAsync(bool fullSynchro = false)
        {
            await this.userSynchronizeDomainService.SynchronizeFromADGroupAsync(fullSynchro);
        }

        /// <inheritdoc cref="IUserAppService.UpdateLastLoginDateAndActivate"/>
        public async Task UpdateLastLoginDateAndActivate(int userId)
        {
            if (userId > 0)
            {
                User entity = await this.Repository.GetEntityAsync(id: userId, queryMode: "NoInclude");
                entity.LastLoginDate = DateTime.Now;
                entity.IsActive = true;

                // this.Repository.Update(entity)
                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IUserAppService.AddInDBAsync"/>
        public async Task AddInDBAsync(IEnumerable<UserFromDirectoryDto> users)
        {
            foreach (var user in users)
            {
                await this.GetCreateUserInfoAsync(user.Sid);
            }
        }

        /// <summary>
        /// Selects the default language.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        public void SelectDefaultLanguage(UserInfoDto userInfo)
        {
            userInfo.Language = this.configuration.Cultures.Where(w => w.IsDefaultForCountryCodes.Any(cc => cc == userInfo.Country))
                .Select(s => s.Code)
                .FirstOrDefault();

            if (userInfo.Language == null)
            {
                // Select the default culture
                userInfo.Language = this.configuration.Cultures.Where(w => w.IsDefaultForCountryCodes.Any(cc => cc == "default"))
                    .Select(s => s.Code)
                    .FirstOrDefault();
            }
        }

        /// <inheritdoc cref="IUserAppService.GetCsvAsync"/>
        public virtual async Task<byte[]> GetCsvAsync(PagingFilterFormatDto filters)
        {
            return await this.GetCsvAsync<UserDto, UserMapper, PagingFilterFormatDto>(filters: filters);
        }
    }
}
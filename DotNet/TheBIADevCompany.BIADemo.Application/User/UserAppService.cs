// <copyright file="UserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
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

        private readonly IUserIdentityKeyDomainService userIdentityKeyDomainService;

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
        /// <param name="userIdentityKeyDomainService">The user Identity Key Domain Service.</param>
        public UserAppService(
            ITGenericRepository<User, int> repository,
            IUserSynchronizeDomainService userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ILogger<UserAppService> logger,
            IIdentityProviderRepository identityProviderRepository,
            IUserIdentityKeyDomainService userIdentityKeyDomainService)
            : base(repository)
        {
            this.userSynchronizeDomainService = userSynchronizeDomainService;
            this.configuration = configuration.Value;
            this.userDirectoryHelper = userDirectoryHelper;
            this.logger = logger;
            this.identityProviderRepository = identityProviderRepository;
            this.userIdentityKeyDomainService = userIdentityKeyDomainService;
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<User>(u => u.IsActive));
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

        /// <inheritdoc cref="IUserAppService.AddUserFromUserDirectoryAsync"/>
        public async Task<User> AddUserFromUserDirectoryAsync(string identityKey, UserFromDirectory userFromDirectory)
        {
            if (userFromDirectory != null)
            {
                User user = new User();
                UserFromDirectory.UpdateUserFieldFromDirectory(user, userFromDirectory);

                var func = this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(identityKey).Compile();
                if (!func(user))
                {
                    throw new ForbiddenException("The identityKey in ldap do not correspond to identityKey in identity.");
                }

                this.Repository.Add(user);
                await this.Repository.UnitOfWork.CommitAsync();

                return user;
            }

            return null;
        }

        /// <inheritdoc cref="IUserAppService.CreateUserInfo"/>
        public UserInfoDto CreateUserInfo(User user)
        {
            if (user != null)
            {
                UserInfoDto userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Country = user.Country,
                    IsActive = user.IsActive,
                };
                return userInfo;
            }

            return null;
        }

        /// <inheritdoc cref="IUserAppService.GetUserInfoAsync"/>
        public async Task<UserInfoDto> GetUserInfoAsync(string identityKey)
        {
            return await this.Repository.GetResultAsync(UserSelectBuilder.SelectUserInfo(), filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(identityKey));
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

        /// <inheritdoc cref="IUserAppService.GetAllIdpUserAsync"/>
        public async Task<IEnumerable<UserFromDirectoryDto>> GetAllIdpUserAsync(string filter, int first = 0, int max = 10)
        {
            List<UserFromDirectory> userFromDirectories = await this.identityProviderRepository.SearchUserAsync(filter, first, max);
            return userFromDirectories.Select(UserFromDirectoryMapper.EntityToDto());
        }

        /// <inheritdoc cref="IUserAppService.AddByIdentityKeyAsync"/>
        public async Task<ResultAddUsersFromDirectoryDto> AddByIdentityKeyAsync(UserDto userDto)
        {
            UserFromDirectoryDto userFromDirectoryDto = new UserFromDirectoryDto();
            userFromDirectoryDto.IdentityKey = this.userIdentityKeyDomainService.GetDtoIdentityKey(userDto);
            List<UserFromDirectoryDto> users = new List<UserFromDirectoryDto>();
            users.Add(userFromDirectoryDto);
            var result = await this.AddFromDirectory(users);
            if (result.UsersAddedDtos.Count > 0)
            {
                foreach (var user in result.UsersAddedDtos)
                {
                    userDto.Id = user.Id;
                    await this.UpdateAsync<UserDto, UserMapper>(userDto, mapperMode: "Roles");
                }
            }

            return result;
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
                result.Errors = await this.userDirectoryHelper.AddUsersInGroup(users, "User");
                try
                {
                    await this.SynchronizeWithADAsync();
                    List<string> usersIdentityKey = users.Select(u => this.userIdentityKeyDomainService.GetDirectoryIdentityKey(u)).ToList();
                    result.UsersAddedDtos = (await this.Repository.GetAllEntityAsync(filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(usersIdentityKey))).Select(entity => new OptionDto
                    {
                        Id = entity.Id,
                        Display = entity.FirstName + " " + entity.LastName + " (" + entity.Login + ")",
                    }).ToList();
                }
                catch (Exception ex)
                {
                    string msg = "Error during synchronize. Retry Synchronize.";
                    this.logger.LogError(ex, msg);
                    result.Errors.Add(msg);
                }
            }
            else
            {
                List<User> usersAdded = new List<User>();
                foreach (var userFormDirectoryDto in users)
                {
                    try
                    {
                        var foundUser = (await this.Repository.GetAllEntityAsync(filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFormDirectoryDto)))).FirstOrDefault();
                        UserFromDirectory userFormDirectory = await this.userDirectoryHelper.ResolveUser(userFormDirectoryDto);
                        if (userFormDirectory != null)
                        {
                            var addedUser = this.userSynchronizeDomainService.AddOrActiveUserFromDirectory(userFormDirectory, foundUser);

                            if (addedUser != null)
                            {
                                usersAdded.Add(addedUser);
                                await this.Repository.UnitOfWork.CommitAsync();
                            }
                            else
                            {
                                result.Errors.Add(userFormDirectoryDto.DisplayName);
                            }
                        }
                        else
                        {
                            result.Errors.Add(userFormDirectoryDto.IdentityKey);
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = userFormDirectoryDto.DisplayName;
                        this.logger.LogError(ex, msg);
                        result.Errors.Add(msg);
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

                List<UserFromDirectoryDto> notRemovedUser = await this.userDirectoryHelper.RemoveUsersInGroup(new List<UserFromDirectoryDto>() { new UserFromDirectoryDto() { DisplayName = user.Display(), IdentityKey = this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user) } }, "User");

                try
                {
                    await this.SynchronizeWithADAsync();
                }
                catch (Exception ex)
                {
                    string msg = "Error during synchronize. Retry Synchronize.";
                    this.logger.LogError(ex, msg);
                    return msg;
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
        public async Task UpdateLastLoginDateAndActivate(int userId, bool activate)
        {
            if (userId > 0)
            {
                User entity = await this.Repository.GetEntityAsync(id: userId, queryMode: "NoInclude");
                entity.LastLoginDate = DateTime.Now;
                entity.IsActive = activate;

                // this.Repository.Update(entity)
                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Selects the default language.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        public void SelectDefaultLanguage(UserInfoDto userInfo)
        {
            userInfo.Language = this.configuration.Cultures.Where(w => Array.Exists(w.IsDefaultForCountryCodes, cc => cc == userInfo.Country))
                .Select(s => s.Code)
                .FirstOrDefault();

            if (userInfo.Language == null)
            {
                // Select the default culture
                userInfo.Language = this.configuration.Cultures.Where(w => Array.Exists(w.AcceptedCodes, cc => cc == "default"))
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
// <copyright file="BaseUserAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Mappers;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using BIA.Net.Core.Domain.User.Specifications;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using static BIA.Net.Core.Common.BiaRights;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserMapper">The type of user mapper.</typeparam>
    public class BaseUserAppService<TUserDto, TUser, TUserMapper> : CrudAppServiceBase<TUserDto, TUser, int, PagingFilterFormatDto, TUserMapper>, IBaseUserAppService<TUserDto, TUser>
        where TUserDto : BaseUserDto, new()
        where TUser : BaseUser, IEntity<int>, new()
        where TUserMapper : BaseUserMapper<TUserDto, TUser>
    {
        /// <summary>
        /// The user synchronize domain service.
        /// </summary>
        private readonly IUserSynchronizeDomainService<TUser> userSynchronizeDomainService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The user default team repository.
        /// </summary>
        private readonly ITGenericRepository<UserDefaultTeam, int> userDefaultTeamRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BaseUserAppService<TUserDto, TUser, TUserMapper>> logger;

        private readonly IIdentityProviderRepository identityProviderRepository;

        private readonly IUserIdentityKeyDomainService<TUser> userIdentityKeyDomainService;

        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserAppService{TUserDto, TUser, TUserMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userSynchronizeDomainService">The user synchronize domain service.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="userDefaultTeamRepository">The user team default repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        /// <param name="userIdentityKeyDomainService">The user Identity Key Domain Service.</param>
        /// <param name="principal">The principal.</param>
        public BaseUserAppService(
            ITGenericRepository<TUser, int> repository,
            IUserSynchronizeDomainService<TUser> userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ITGenericRepository<UserDefaultTeam, int> userDefaultTeamRepository,
            ILogger<BaseUserAppService<TUserDto, TUser, TUserMapper>> logger,
            IIdentityProviderRepository identityProviderRepository,
            IUserIdentityKeyDomainService<TUser> userIdentityKeyDomainService,
            IPrincipal principal)
            : base(repository)
        {
            this.userSynchronizeDomainService = userSynchronizeDomainService;
            this.configuration = configuration.Value;
            this.userDirectoryHelper = userDirectoryHelper;
            this.userDefaultTeamRepository = userDefaultTeamRepository;
            this.logger = logger;
            this.identityProviderRepository = identityProviderRepository;
            this.userIdentityKeyDomainService = userIdentityKeyDomainService;
            this.principal = principal as BiaClaimsPrincipal;
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<TUser>(u => u.IsActive));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OptionDto>> GetAllOptionsAsync(string filter = null)
        {
            Specification<TUser> specification = null;
            if (!string.IsNullOrEmpty(filter))
            {
                specification = UserSpecification<TUser>.Search(filter);
            }

            return await this.GetAllAsync<OptionDto, UserOptionMapper<TUser>>(specification: specification, queryOrder: new QueryOrder<TUser>().OrderBy(o => o.LastName).ThenBy(o => o.FirstName));
        }

        /// <inheritdoc cref="IBaseUserAppService.AddUserFromUserDirectoryAsync"/>
        public async Task<TUser> AddUserFromUserDirectoryAsync(string identityKey, UserFromDirectory userFromDirectory)
        {
            if (userFromDirectory != null)
            {
                TUser user = new TUser();
                this.userSynchronizeDomainService.UpdateUserFieldFromDirectory(user, userFromDirectory);

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

        /// <inheritdoc cref="IBaseUserAppService.CreateUserInfo"/>
        public UserInfoDto CreateUserInfo(TUser user)
        {
            if (user != null)
            {
                UserInfoDto userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                };
                return userInfo;
            }

            return null;
        }

        /// <inheritdoc cref="IBaseUserAppService.GetUserInfoAsync"/>
        public async Task<UserInfoDto> GetUserInfoAsync(string identityKey)
        {
            return await this.Repository.GetResultAsync(UserSelectBuilder<TUser>.SelectUserInfo(), filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(identityKey));
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetAllLdapUsersDomains()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this.configuration.Authentication.LdapDomains.Where(d => d.ContainsUser).Select(d => d.Name).ToList();
        }

        /// <inheritdoc cref="IBaseUserAppService.GetAllADUserAsync"/>
        public async Task<IEnumerable<UserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null, int max = 10)
        {
            return await Task.FromResult(this.userDirectoryHelper.SearchUsers(filter, ldapName, max).OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                .Select(UserFromDirectoryMapper.EntityToDto())
                .ToList());
        }

        /// <inheritdoc cref="IBaseUserAppService.GetAllIdpUserAsync"/>
        public async Task<IEnumerable<UserFromDirectoryDto>> GetAllIdpUserAsync(string filter, int first = 0, int max = 10)
        {
            string formattedFilter = "*" + filter.Replace(" ", " *");
            List<UserFromDirectory> userFromDirectories = await this.identityProviderRepository.SearchUserAsync(formattedFilter, first, max);
            return userFromDirectories.Select(UserFromDirectoryMapper.EntityToDto());
        }

        /// <inheritdoc cref="IBaseUserAppService.AddByIdentityKeyAsync"/>
        public async Task<ResultAddUsersFromDirectoryDto> AddByIdentityKeyAsync(TUserDto userDto)
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
                    await this.UpdateAsync<TUserDto, TUserMapper>(userDto, mapperMode: "RolesInit");
                }
            }

            return result;
        }

        /// <inheritdoc cref="IBaseUserAppService.AddFromDirectory"/>
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
                        Display = entity.LastName + " " + entity.FirstName + " (" + entity.Login + ")",
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
                List<TUser> usersAdded = new List<TUser>();
                foreach (var userFormDirectoryDto in users)
                {
                    try
                    {
                        TUser foundUser = (await this.Repository.GetAllEntityAsync(filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFormDirectoryDto)))).FirstOrDefault();

                        UserFromDirectory userFormDirectory = null;

                        if (this.configuration?.Authentication?.Keycloak?.IsActive == true)
                        {
                            userFormDirectory = await this.identityProviderRepository.FindUserAsync(userFormDirectoryDto.IdentityKey);
                        }
                        else
                        {
                            userFormDirectory = await this.userDirectoryHelper.ResolveUser(userFormDirectoryDto);
                        }

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
                    Display = entity.LastName + " " + entity.FirstName + " (" + entity.Login + ")",
                }).ToList();
            }

            return result;
        }

        /// <inheritdoc cref="IBaseUserAppService.RemoveInGroupAsync"/>
        public async Task<string> RemoveInGroupAsync(int id)
        {
            var ldapGroups = this.userDirectoryHelper.GetLdapGroupsForRole("User");
            var user = await this.Repository.GetEntityAsync(id: id, includes: [x => x.Roles]);
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

        /// <inheritdoc cref="IBaseUserAppService.SynchronizeWithADAsync"/>
        public async Task SynchronizeWithADAsync(bool fullSynchro = false)
        {
            await this.userSynchronizeDomainService.SynchronizeFromADGroupAsync(fullSynchro);
        }

        /// <inheritdoc cref="IBaseUserAppService.SynchronizeWithIdpAsync"/>
        public async Task SynchronizeWithIdpAsync()
        {
            await this.userSynchronizeDomainService.SynchronizeFromIdpAsync();
        }

        /// <inheritdoc cref="IBaseUserAppService.UpdateLastLoginDateAndActivate"/>
        public async Task UpdateLastLoginDateAndActivate(int userId, bool activate)
        {
            if (userId > 0)
            {
                TUser entity = await this.Repository.GetEntityAsync(id: userId, queryMode: "NoInclude");
                entity.LastLoginDate = DateTime.Now;
                entity.IsActive = activate;

                // this.Repository.Update(entity)
                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IBaseUserAppService.SaveAsync"/>
        public async Task<string> SaveAsync(List<TUserDto> userDtos)
        {
            StringBuilder strBldr = new StringBuilder();

            int nbAdded = default;
            int nbUpdated = default;
            int nbError = default;

            if (userDtos?.Any() == true)
            {
                IEnumerable<string> currentUserPermissions = this.principal.GetUserPermissions();
                bool canAdd = currentUserPermissions?.Any(x => x == Users.Add) == true;
                bool canUpdate = currentUserPermissions?.Any(x => x == Users.UpdateRoles) == true;

                var exceptions = new List<Exception>();

                if (!canAdd && userDtos.Exists(u => u.DtoState == DtoState.Added))
                {
                    strBldr.AppendLine("No permission to add users.");
                    nbError++;
                }

                if (!canUpdate && userDtos.Exists(u => u.DtoState == DtoState.Modified))
                {
                    strBldr.AppendLine("No permission to update users.");
                    nbError++;
                }

                if (userDtos.Exists(u => u.DtoState == DtoState.Deleted))
                {
                    strBldr.AppendLine("No permission to delete users.");
                    nbError++;
                }

                foreach (TUserDto userDto in userDtos)
                {
                    try
                    {
                        if (canAdd && userDto.DtoState == DtoState.Added)
                        {
                            ResultAddUsersFromDirectoryDto result = await this.AddByIdentityKeyAsync(userDto);
                            this.Repository.UnitOfWork.Reset();

                            if (result?.Errors?.Any() == true)
                            {
                                result.Errors.ForEach(error => strBldr.AppendLine(error));
                                nbError++;
                            }
                            else if (result?.UsersAddedDtos?.Any() == true)
                            {
                                nbAdded++;
                            }
                        }
                        else if (canUpdate && userDto.DtoState == DtoState.Modified)
                        {
                            await this.UpdateAsync<TUserDto, TUserMapper>(userDto, mapperMode: "Roles");
                            this.Repository.UnitOfWork.Reset();
                            nbUpdated++;
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        nbError++;
                    }
                }

                if (nbError > 0)
                {
                    strBldr.Insert(0, $"Added: {nbAdded}, Updated: {nbUpdated}, Error{(nbError > 1 ? "s" : null)}: {nbError}{Environment.NewLine}");
                    string errorMessage = strBldr.ToString();
                    var aggregateException = new AggregateException(exceptions);
                    this.logger.LogError(message: errorMessage, exception: aggregateException);
                    return errorMessage;
                }
            }

            return null;
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

        /// <inheritdoc cref="IBaseUserAppService.GetCsvAsync"/>
        public virtual async Task<byte[]> GetCsvAsync(PagingFilterFormatDto filters)
        {
            return await this.GetCsvAsync<TUserDto, TUserMapper, PagingFilterFormatDto>(filters: filters);
        }

        /// <inheritdoc cref="IBaseUserAppService.SetDefaultSite"/>
        public async Task SetDefaultTeamAsync(int teamId, int teamTypeId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0)
            {
                var userDefaultTeams = await this.userDefaultTeamRepository.GetAllEntityAsync(
                    filter: x => x.UserId == userId && x.Team.TeamTypeId == teamTypeId,
                    includes: [u => u.Team]);

                foreach (var userDefaultTeam in userDefaultTeams.Where(x => x.TeamId != teamId))
                {
                    this.userDefaultTeamRepository.Remove(userDefaultTeam);
                }

                if (!userDefaultTeams.Any(x => x.TeamId == teamId))
                {
                    this.userDefaultTeamRepository.Add(new UserDefaultTeam { TeamId = teamId, UserId = userId });
                }

                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IBaseUserAppService.ResetDefaultSite"/>
        public async Task ResetDefaultTeamAsync(int teamTypeId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0)
            {
                var userDefaultTeams = await this.userDefaultTeamRepository.GetAllEntityAsync(
                    filter: x => x.UserId == userId && x.Team.TeamTypeId == teamTypeId,
                    includes: [u => u.Team]);

                foreach (var userDefaultTeam in userDefaultTeams)
                {
                    this.userDefaultTeamRepository.Remove(userDefaultTeam);
                }

                await this.Repository.UnitOfWork.CommitAsync();
            }
        }
    }
}
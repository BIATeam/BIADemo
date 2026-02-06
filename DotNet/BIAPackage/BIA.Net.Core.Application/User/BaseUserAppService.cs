// <copyright file="BaseUserAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
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
    using BIA.Net.Core.Domain.User.Services;
    using BIA.Net.Core.Domain.User.Specifications;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using static BIA.Net.Core.Common.BiaRights;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserMapper">The type of user mapper.</typeparam>
    /// <typeparam name="TUserFromDirectoryDto">The type of user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    public abstract class BaseUserAppService<TUserDto, TUser, TUserMapper, TUserFromDirectoryDto, TUserFromDirectory> : CrudAppServiceBase<TUserDto, TUser, int, PagingFilterFormatDto, TUserMapper>, IBaseUserAppService<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory>
        where TUserDto : BaseUserDto, new()
        where TUser : BaseEntityUser, IEntity<int>, new()
        where TUserMapper : BaseUserMapper<TUserDto, TUser>
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : class, IUserFromDirectory, new()
    {
        /// <summary>
        /// The user synchronize domain service.
        /// </summary>
        private readonly IBaseUserSynchronizeDomainService<TUser, TUserFromDirectory> userSynchronizeDomainService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The user default team repository.
        /// </summary>
        private readonly ITGenericRepository<UserDefaultTeam, int> userDefaultTeamRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BaseUserAppService<TUserDto, TUser, TUserMapper, TUserFromDirectoryDto, TUserFromDirectory>> logger;

        private readonly IIdentityProviderRepository<TUserFromDirectory> identityProviderRepository;

        private readonly IUserIdentityKeyDomainService userIdentityKeyDomainService;

        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserAppService{TUserDto, TUser, TUserMapper, TUserFromDirectoryDto, TUserFromDirectory}"/> class.
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
        protected BaseUserAppService(
            ITGenericRepository<TUser, int> repository,
            IBaseUserSynchronizeDomainService<TUser, TUserFromDirectory> userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> userDirectoryHelper,
            ITGenericRepository<UserDefaultTeam, int> userDefaultTeamRepository,
            ILogger<BaseUserAppService<TUserDto, TUser, TUserMapper, TUserFromDirectoryDto, TUserFromDirectory>> logger,
            IIdentityProviderRepository<TUserFromDirectory> identityProviderRepository,
            IUserIdentityKeyDomainService userIdentityKeyDomainService,
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

        /// <inheritdoc />
        public async Task<IEnumerable<OptionDto>> GetAllOptionsAsync(string filter = null)
        {
            Specification<TUser> specification = null;
            if (!string.IsNullOrEmpty(filter))
            {
                specification = UserSpecification<TUser>.Search(filter);
            }

            return await this.GetAllGenericAsync<OptionDto, UserOptionMapper<TUser>>(specification: specification, queryOrder: new QueryOrder<TUser>().OrderBy(o => o.LastName).ThenBy(o => o.FirstName));
        }

        /// <inheritdoc />
        public async Task<TUser> AddUserFromUserDirectoryAsync(string identityKey, TUserFromDirectory userFromDirectory)
        {
            if (userFromDirectory != default(TUserFromDirectory))
            {
                TUser user = new TUser();
                this.userSynchronizeDomainService.UpdateUserFieldFromDirectory(user, userFromDirectory);

                var func = this.userIdentityKeyDomainService.CheckDatabaseIdentityKey<TUser>(identityKey).Compile();
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

        /// <inheritdoc />
        public UserInfoFromDBDto CreateUserInfo(TUser user)
        {
            if (user != null)
            {
                UserInfoFromDBDto userInfo = new UserInfoFromDBDto
                {
                    Id = user.Id,
                    IdentityKey = this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                };
                return userInfo;
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<UserInfoFromDBDto> GetUserInfoAsync(string identityKey)
        {
            return await this.Repository.GetResultAsync(UserSelectBuilder<TUser>.SelectUserInfo(this.userIdentityKeyDomainService), filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey<TUser>(identityKey));
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetAllLdapUsersDomains()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this.configuration.Authentication?.LdapDomains.Where(d => d.ContainsUser).Select(d => d.Name).ToList();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TUserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null, int max = 10)
        {
            IUserFromDirectoryMapper<TUserFromDirectoryDto, TUserFromDirectory> userFromDirectoryMapper = this.Repository.ServiceProvider.GetService<IUserFromDirectoryMapper<TUserFromDirectoryDto, TUserFromDirectory>>();
            return await Task.FromResult(this.userDirectoryHelper.SearchUsers(filter, ldapName, max).OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                .Select(userFromDirectoryMapper.EntityToDto())
                .ToList());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TUserFromDirectoryDto>> GetAllIdpUserAsync(string filter, int first = 0, int max = 10)
        {
            IUserFromDirectoryMapper<TUserFromDirectoryDto, TUserFromDirectory> userFromDirectoryMapper = this.Repository.ServiceProvider.GetService<IUserFromDirectoryMapper<TUserFromDirectoryDto, TUserFromDirectory>>();
            string formattedFilter = "*" + filter.Replace(" ", " *");
            List<TUserFromDirectory> userFromDirectories = await this.identityProviderRepository.SearchUserAsync(formattedFilter, first, max);
            return userFromDirectories.Select(userFromDirectoryMapper.EntityToDto());
        }

        /// <inheritdoc />
        public async Task<ResultAddUsersFromDirectoryDto> AddByIdentityKeyAsync(TUserDto userDto)
        {
            TUserFromDirectoryDto userFromDirectoryDto = new TUserFromDirectoryDto();
            userFromDirectoryDto.IdentityKey = this.userIdentityKeyDomainService.GetDtoIdentityKey(userDto);
            List<TUserFromDirectoryDto> users = new List<TUserFromDirectoryDto>();
            users.Add(userFromDirectoryDto);
            var result = await this.AddFromDirectory(users);
            if (result.UsersAddedDtos.Count > 0)
            {
                foreach (var user in result.UsersAddedDtos)
                {
                    userDto.Id = user.Id;
                    await this.UpdateGenericAsync<TUserDto, TUserMapper>(userDto, mapperMode: "RolesInit");
                }
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<ResultAddUsersFromDirectoryDto> AddFromDirectory(IEnumerable<TUserFromDirectoryDto> users)
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
                    List<string> usersIdentityKey = users.Select(u => u.IdentityKey).ToList();
                    result.UsersAddedDtos = (await this.Repository.GetAllEntityAsync(filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey<TUser>(usersIdentityKey))).Select(entity => new OptionDto
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
                        TUser foundUser = (await this.Repository.GetAllEntityAsync(filter: this.userIdentityKeyDomainService.CheckDatabaseIdentityKey<TUser>(userFormDirectoryDto.IdentityKey))).FirstOrDefault();

                        TUserFromDirectory userFormDirectory;

                        if (this.configuration?.Authentication?.Keycloak?.IsActive == true)
                        {
                            userFormDirectory = await this.identityProviderRepository.FindUserAsync(userFormDirectoryDto.IdentityKey);
                        }
                        else
                        {
                            userFormDirectory = await this.userDirectoryHelper.ResolveUser(userFormDirectoryDto);
                        }

                        if (userFormDirectory != default(TUserFromDirectory))
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

        /// <inheritdoc />
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

                List<TUserFromDirectoryDto> notRemovedUser = await this.userDirectoryHelper.RemoveUsersInGroup(new List<TUserFromDirectoryDto>() { new TUserFromDirectoryDto() { DisplayName = user.Display(), IdentityKey = this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user) } }, "User");

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

        /// <inheritdoc />
        public async Task SynchronizeWithADAsync(bool fullSynchro = false)
        {
            await this.userSynchronizeDomainService.SynchronizeFromADGroupAsync(fullSynchro);
        }

        /// <inheritdoc />
        public async Task SynchronizeWithIdpAsync()
        {
            await this.userSynchronizeDomainService.SynchronizeFromIdpAsync();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<string> SaveAsync(List<TUserDto> userDtos)
        {
            StringBuilder strBldr = new StringBuilder();

            int nbAdded = default;
            int nbUpdated = default;
            int nbError = default;

            if (userDtos?.Any() == true)
            {
                IEnumerable<string> currentUserPermissions = this.principal.GetUserPermissions();
                bool canAdd = currentUserPermissions?.Any(x => x == nameof(BiaPermissionId.User_Add)) == true;
                bool canUpdate = currentUserPermissions?.Any(x => x == nameof(BiaPermissionId.User_UpdateRoles)) == true;

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
                            await this.UpdateGenericAsync<TUserDto, TUserMapper>(userDto, mapperMode: "Roles");
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
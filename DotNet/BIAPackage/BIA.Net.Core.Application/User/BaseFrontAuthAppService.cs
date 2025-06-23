// <copyright file="BaseFrontAuthAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TEnumRoleId">The type for enum Role Id.</typeparam>
    /// <typeparam name="TEnumTeamTypeId">The type for enum Team Type Id.</typeparam>
    /// <typeparam name="TUserFromDirectoryDto">The type of user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    /// <typeparam name="TAdditionalInfoDto">The type of additional info dto.</typeparam>
    /// <typeparam name="TUserDataDto">The type of user data dto.</typeparam>
    public abstract class BaseFrontAuthAppService<TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId, TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto> : BaseAuthAppService<TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto>, IBaseFrontAuthAppService<TAdditionalInfoDto>
        where TUserDto : BaseUserDto, new()
        where TUser : BaseEntityUser, IEntity<int>, new()
        where TEnumRoleId : struct, Enum
        where TEnumTeamTypeId : struct, Enum
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : class, IUserFromDirectory, new()
        where TAdditionalInfoDto : BaseAdditionalInfoDto, new()
        where TUserDataDto : BaseUserDataDto, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFrontAuthAppService{TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId, TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto}" /> class.
        /// </summary>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userPermissionDomainService">The user permission domain service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="biaNetconfiguration">The bia netconfiguration.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="ldapRepositoryHelper">The LDAP repository helper.</param>
        protected BaseFrontAuthAppService(
            IBaseUserAppService<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory> userAppService,
            IBaseTeamAppService<TEnumTeamTypeId> teamAppService,
            IRoleAppService roleAppService,
            IIdentityProviderRepository<TUserFromDirectory> identityProviderRepository,
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            ILogger<BaseFrontAuthAppService<TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId, TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto>> logger,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> userDirectoryHelper,
            ILdapRepositoryHelper ldapRepositoryHelper)
            : base(jwtFactory, principal, userPermissionDomainService, logger, configuration, biaNetconfiguration, userDirectoryHelper, ldapRepositoryHelper)
        {
            this.UserAppService = userAppService;
            this.TeamAppService = teamAppService;
            this.RoleAppService = roleAppService;
            this.IdentityProviderRepository = identityProviderRepository;
            this.RolesConfiguration = biaNetconfiguration.Value.Roles;
        }

        /// <summary>
        /// The role section in the BiaNet configuration.
        /// </summary>
        protected IEnumerable<BIA.Net.Core.Common.Configuration.Role> RolesConfiguration { get; }

        /// <summary>
        /// The identity provider repository.
        /// </summary>
        protected IIdentityProviderRepository<TUserFromDirectory> IdentityProviderRepository { get; }

        /// <summary>
        /// The user application service.
        /// </summary>
        protected IBaseUserAppService<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory> UserAppService { get; }

        /// <summary>
        /// The team application service.
        /// </summary>
        protected IBaseTeamAppService<TEnumTeamTypeId> TeamAppService { get; }

        /// <summary>
        /// The role application service.
        /// </summary>
        protected IRoleAppService RoleAppService { get; }

        /// <inheritdoc cref="IAuthAppService.LoginOnTeamsAsync"/>
        public virtual async Task<AuthInfoDto<TAdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
        {
            // Check if current user is authenticated
            this.CheckIsAuthenticated();

            AuthInfoDto<TAdditionalInfoDto> authInfo = await this.GetLoginToken(loginParam, true, teamsConfig);

            if (!string.IsNullOrWhiteSpace(loginParam.BaseUserIdentity) && Application.Authentication.JwtFactory.HasRole(authInfo.Token, BiaRights.Impersonation.ConnectionRights))
            {
                return await this.GetLoginToken(loginParam, false, teamsConfig);
            }
            else
            {
                return authInfo;
            }
        }

        /// <summary>
        /// Gets the role ids.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Role ids.</returns>
        protected static List<int> GetRoleIds(List<string> roles)
        {
            List<int> roleIds = new List<int>();
            foreach (string role in roles)
            {
                if (Enum.TryParse<TEnumRoleId>(role, out var roleId) && !roleIds.Contains(Convert.ToInt32(roleId)))
                {
                    roleIds.Add(Convert.ToInt32(roleId));
                }

                if (Enum.TryParse<BiaRoleId>(role, out var biaRoleId) && !roleIds.Contains(Convert.ToInt32(biaRoleId)))
                {
                    roleIds.Add(Convert.ToInt32(biaRoleId));
                }
            }

            return roleIds;
        }

        /// <summary>
        /// Gets the login token.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="withCredentials">if set to <c>true</c> [with credentials].</param>
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <typeparam name="TAdditionalInfoDto">The type of AdditionalInfoDto.</typeparam>
        /// <typeparam name="TUserDataDto">The type of UserDataDto.</typeparam>
        /// <returns>Return a token to authenticate user with permition.</returns>
        protected virtual async Task<AuthInfoDto<TAdditionalInfoDto>> GetLoginToken(LoginParamDto loginParam, bool withCredentials, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
        {
            // Get informations in Claims
            string sid = this.GetSid();
            string domain = this.GetDomain();
            string identityKey = withCredentials ? this.GetIdentityKey() : loginParam.BaseUserIdentity;

            // Get UserInfo from database
            UserInfoFromDBDto userInfoFromDB = await this.GetUserInfoFromDB(loginParam, identityKey);

            // Get Global Roles
            List<string> globalRoles = await this.GetGlobalRolesAsync(sid: sid, domain: domain, userInfo: userInfoFromDB, withCredentials);

            // If the user has the User role
            // Automatic creation from ldap, usefull if user do not need fine Role on team.
            // Else update last login date and activate the user.
            userInfoFromDB = await this.CreateOrUpdateUserInDatabase(sid, identityKey, userInfoFromDB, globalRoles);

            // Get User AppRoot Roles
            if (userInfoFromDB?.Id > 0 && globalRoles.Contains(BiaConstants.Role.User))
            {
                IEnumerable<string> userAppRootRoles = await this.RoleAppService.GetUserRolesAsync(userInfoFromDB.Id);
                globalRoles.AddRange(userAppRootRoles);
            }

            List<int> roleIds = GetRoleIds(globalRoles);

            // Get Permissions
            List<string> userPermissions = this.UserPermissionDomainService.TranslateRolesInPermissions(globalRoles, loginParam.LightToken);

            IEnumerable<BaseDtoVersionedTeam> allTeams = [];
            TUserDataDto userData = this.CreateUserData(userInfoFromDB);

            // Get Fine Grained Permissions
            if (loginParam.FineGrainedPermission && userInfoFromDB?.Id > 0)
            {
                // Get All Teams
                allTeams = await this.TeamAppService.GetAllAsync(teamsConfig, userInfoFromDB.Id, userPermissions);

                // Get Fine Grained Roles
                List<string> fineGrainedRoles = await this.GetFineRolesAsync(loginParam, userData, userInfoFromDB.Id, allTeams, teamsConfig);
                List<int> fineGrainedRoleIds = GetRoleIds(fineGrainedRoles);
                roleIds = roleIds.Union(fineGrainedRoleIds).ToList();

                // Translate Roles in Permissions
                List<string> fineGrainedUserPermissions = this.UserPermissionDomainService.TranslateRolesInPermissions(fineGrainedRoles, loginParam.LightToken);

                // Concat global permissions and fine grained permissions
                userPermissions = userPermissions.Union(fineGrainedUserPermissions).ToList();
            }

            // Check User Permissions
            this.CheckUserPermissions(userPermissions);

            // Sort User Permissions
            userPermissions.Sort();

            // Create Token Dto
            TokenDto<TUserDataDto> tokenDto = new ()
            {
                IdentityKey = identityKey,
                Id = (userInfoFromDB?.Id).GetValueOrDefault(),
                RoleIds = roleIds,
                Permissions = userPermissions,
                UserData = userData,
            };

            // Get AdditionalInfoDto
            TAdditionalInfoDto additionalInfo = this.GetAdditionalInfo(loginParam, allTeams, userData, teamsConfig);

            // Create AuthInfo
            AuthInfoDto<TAdditionalInfoDto> authInfo = await this.JwtFactory.GenerateAuthInfoAsync(tokenDto, additionalInfo, loginParam);

            return authInfo;
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="identityKey">The identity key.</param>
        /// <returns>A UserInfo Dto.</returns>
        protected virtual async Task<UserInfoFromDBDto> GetUserInfoFromDB(LoginParamDto loginParam, string identityKey)
        {
            // Get userInfo if needed (it requires an user in database)
            UserInfoFromDBDto userInfo = null;

            if (loginParam.FineGrainedPermission || loginParam.AdditionalInfos || this.UseUserRole())
            {
                userInfo = await this.UserAppService.GetUserInfoAsync(identityKey);
            }

            // If the user does not exist in the database
            // We create a UserInfoDto object from principal
            userInfo ??= new UserInfoFromDBDto
            {
                Id = 0,
                IdentityKey = identityKey,
                IsActive = false,
                FirstName = this.ClaimsPrincipal.GetClaimValue(ClaimTypes.GivenName),
                LastName = this.ClaimsPrincipal.GetClaimValue(ClaimTypes.Surname),
            };

            return userInfo;
        }

        /// <summary>
        /// Gets the identity key.
        /// </summary>
        /// <returns>The identity key.</returns>
        protected virtual string GetIdentityKey()
        {
            throw new NotImplementedException("The function GetIdentityKey should be define in the project.");
        }

        /// <summary>
        /// Checks if the rolesConfiguration contains the 'User' role.
        /// </summary>
        /// <returns>Return true if the rolesConfiguration contains the 'User' role.</returns>
        protected virtual bool UseUserRole()
        {
            return this.RolesConfiguration != null && this.RolesConfiguration.Any(r =>
                r.Label == BiaConstants.Role.User);
        }

        /// <summary>
        /// Gets the additional information.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="allTeams">All teams.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <typeparam name="TAdditionalInfoDto">The type of AdditionalInfoDto.</typeparam>
        /// <typeparam name="TUserDataDto">The type of UserDataDto.</typeparam>
        /// <returns>A AdditionalInfo Dto.</returns>
        protected virtual TAdditionalInfoDto GetAdditionalInfo(LoginParamDto loginParam, IEnumerable<BaseDtoVersionedTeam> allTeams, TUserDataDto userData, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
        {
            TAdditionalInfoDto additionalInfo = this.CreateAdditionalInfo();

            if (loginParam.AdditionalInfos)
            {
                var allTeamsFilteredByCurrentParent = allTeams.Where(t => teamsConfig.Exists(tc => tc.TeamTypeId == t.TeamTypeId && (
                    tc.Parents == null
                    ||
                    tc.Parents.Exists(p => userData.CurrentTeams.Any(ct => ct.TeamId == t.ParentTeamId))))).ToList();

                additionalInfo.Teams = [.. allTeamsFilteredByCurrentParent.OrderBy(x => x.Title)];
            }

            return additionalInfo;
        }

        /// <summary>
        /// Creates the or update user in database.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="identityKey">The identity key.</param>
        /// <param name="userInfoFromDB">The user information.</param>
        /// <param name="globalRoles">The global roles.</param>
        /// <returns>A UserInfoDto.</returns>
        protected virtual async Task<UserInfoFromDBDto> CreateOrUpdateUserInDatabase(string sid, string identityKey, UserInfoFromDBDto userInfoFromDB, List<string> globalRoles)
        {
            if (globalRoles.Contains(BiaConstants.Role.User))
            {
                if (!(userInfoFromDB?.Id > 0))
                {
                    // automatic creation from ldap, only use if user do not need fine Role on team.
                    try
                    {
                        TUserFromDirectory userFromDirectory;

                        if (!string.IsNullOrWhiteSpace(sid))
                        {
                            userFromDirectory = await this.UserDirectoryHelper.ResolveUserByIdentityKey(identityKey);
                        }
                        else
                        {
                            userFromDirectory = await this.IdentityProviderRepository.FindUserAsync(identityKey);
                        }

                        if (userFromDirectory != default(TUserFromDirectory))
                        {
                            TUser user = await this.UserAppService.AddUserFromUserDirectoryAsync(identityKey, userFromDirectory);
                            userInfoFromDB = this.UserAppService.CreateUserInfo(user);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.LogError(ex, "Cannot create user... Probably database is read only...");
                    }
                }
                else
                {
                    try
                    {
                        // The date of the last connection is updated in the database
                        await this.UserAppService.UpdateLastLoginDateAndActivate(userInfoFromDB.Id, true);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.LogError(ex, "Cannot update last login date... Probably database is read only...");
                    }
                }
            }

            return userInfoFromDB;
        }

        /// <summary>
        /// Gets the roles asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="userInfoId">The id of the user.</param>
        /// <param name="allTeams">All teams.</param>
        /// <param name="teamsConfig">The teams config.</param>
        /// <typeparam name="TUserDataDto">The type of UserDataDto.</typeparam>
        /// <returns>List of role.</returns>
        protected virtual async Task<List<string>> GetFineRolesAsync(LoginParamDto loginParam, TUserDataDto userData, int userInfoId, IEnumerable<BaseDtoVersionedTeam> allTeams, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
        {
            // the main roles
            var allRoles = new List<string>();

            // get user rights
            if (loginParam.TeamsConfig != null)
            {
                foreach (var loginTeamConfigTeamTypeId in loginParam.TeamsConfig.Select(x => x.TeamTypeId))
                {
                    var teamConfig = teamsConfig.Single(tc => tc.TeamTypeId == loginTeamConfigTeamTypeId);
                    var correspondingTeams = allTeams.Where(t => t.TeamTypeId == loginTeamConfigTeamTypeId);
                    var automaticallySelectedTeam = teamConfig.TeamSelectionMode switch
                    {
                        TeamSelectionMode.None => null,
                        TeamSelectionMode.First => correspondingTeams.FirstOrDefault(),
                        _ => throw new NotImplementedException()
                    };
                    var defaultTeam = correspondingTeams.FirstOrDefault(x => x.IsDefault) ?? automaticallySelectedTeam;

                    CurrentTeamDto teamLogin = null;
                    if (loginParam.IsFirstLogin && defaultTeam != null)
                    {
                        teamLogin = new CurrentTeamDto
                        {
                            TeamTypeId = defaultTeam.TeamTypeId,
                            TeamId = defaultTeam.Id,
                            TeamTitle = defaultTeam.Title,
                            UseDefaultRoles = true,
                            CurrentRoleIds = { },
                        };
                    }
                    else if (!loginParam.IsFirstLogin)
                    {
                        teamLogin = Array.Find(loginParam.CurrentTeamLogins, ct => ct.TeamTypeId == loginTeamConfigTeamTypeId);
                    }

                    if (teamLogin is null)
                    {
                        continue;
                    }

                    var currentTeam = new CurrentTeamDto()
                    {
                        TeamTypeId = teamLogin.TeamTypeId,
                        TeamId = teamLogin.TeamId,
                        TeamTitle = correspondingTeams.FirstOrDefault(s => s.Id == teamLogin.TeamId)?.Title,
                    };

                    if (currentTeam.TeamId <= 0)
                    {
                        continue;
                    }

                    IEnumerable<RoleDto> roles = await this.RoleAppService.GetMemberRolesAsync(currentTeam.TeamId, userInfoId);
                    RoleMode roleMode = Array.Find(loginParam.TeamsConfig, r => r.TeamTypeId == currentTeam.TeamTypeId)?.RoleMode ?? RoleMode.AllRoles;

                    if (roleMode == RoleMode.AllRoles)
                    {
                        currentTeam.CurrentRoleIds = roles.Select(r => r.Id).ToList();
                    }
                    else if (roleMode == RoleMode.SingleRole)
                    {
                        RoleDto role = roles?.OrderByDescending(x => x.IsDefault).FirstOrDefault();
                        if (role != null)
                        {
                            if (teamLogin.CurrentRoleIds != null && teamLogin.CurrentRoleIds.Count == 1 && roles.Any(s => s.Id == teamLogin.CurrentRoleIds[0]))
                            {
                                currentTeam.CurrentRoleIds = new List<int> { teamLogin.CurrentRoleIds[0] };
                            }
                            else
                            {
                                currentTeam.CurrentRoleIds = new List<int> { role.Id };
                            }
                        }
                        else
                        {
                            currentTeam.CurrentRoleIds = new List<int>();
                        }
                    }
                    else
                    {
                        if (roles.Any())
                        {
                            if (!teamLogin.UseDefaultRoles)
                            {
                                List<int> roleIdsToSet = roles.Where(r => teamLogin.CurrentRoleIds != null && teamLogin.CurrentRoleIds.Exists(tr => tr == r.Id)).Select(r => r.Id).ToList();
                                currentTeam.CurrentRoleIds = roleIdsToSet;
                            }
                            else
                            {
                                currentTeam.CurrentRoleIds = roles.Where(x => x.IsDefault).Select(r => r.Id).ToList();
                            }
                        }
                        else
                        {
                            currentTeam.CurrentRoleIds = new List<int>();
                        }
                    }

                    if (allTeams.Any(team => team.Id == currentTeam.TeamId && team.TeamTypeId == currentTeam.TeamTypeId))
                    {
                        userData.CurrentTeams.Add(currentTeam);
                    }

                    // add the sites roles (filter if singleRole mode is used)
                    allRoles.AddRange(roles.Where(r => currentTeam.CurrentRoleIds.Exists(id => id == r.Id)).Select(r => r.Code).ToList());
                    allRoles.Add(teamConfig.RightPrefix + BiaConstants.Role.TeamMemberSuffix);
                    allRoles.Add(teamConfig.RightPrefix + BiaConstants.Role.TeamMemberOfOneSuffix);

                    // add computed roles (can be customized)
                }

                foreach (var teamConfig in teamsConfig)
                {
                    if (teamConfig.Parents == null && allTeams.Any(t => t.TeamTypeId == teamConfig.TeamTypeId))
                    {
                        allRoles.Add(teamConfig.RightPrefix + BiaConstants.Role.TeamMemberOfOneSuffix);
                    }
                }
            }

            return allRoles;
        }
    }
}
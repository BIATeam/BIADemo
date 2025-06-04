// <copyright file="AuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Application.User;
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
    using TheBIADevCompany.BIADemo.Domain.User;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TEnumRoleId">The type for enum Role Id.</typeparam>
    /// <typeparam name="TEnumTeamTypeId">The type for enum Team Type Id.</typeparam>
    public class AuthAppService<TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId> : IAuthAppService
        where TUserDto : BaseUserDto, new()
        where TUser : BaseUser, IEntity<int>, new()
        where TEnumRoleId : struct, Enum
        where TEnumTeamTypeId : struct, Enum
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthAppService<TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId>> logger;

        /// <summary>
        /// The principal.
        /// </summary>
        private readonly BiaClaimsPrincipal claimsPrincipal;

        /// <summary>
        /// The JWT factory.
        /// </summary>
        private readonly IJwtFactory jwtFactory;

        /// <summary>
        /// The user permission domain service.
        /// </summary>
        private readonly IUserPermissionDomainService userPermissionDomainService;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The domain section in the BiaNet configuration.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// The ldap repository service.
        /// </summary>
        private readonly ILdapRepositoryHelper ldapRepositoryHelper;
#if BIA_FRONT_FEATURE

        /// <summary>
        /// The role section in the BiaNet configuration.
        /// </summary>
        private readonly IEnumerable<BIA.Net.Core.Common.Configuration.Role> rolesConfiguration;

        /// <summary>
        /// The identity provider repository.
        /// </summary>
        private readonly IIdentityProviderRepository identityProviderRepository;

        /// <summary>
        /// The user application service.
        /// </summary>
        private readonly IBaseUserAppService<TUserDto, TUser> userAppService;

        /// <summary>
        /// The team application service.
        /// </summary>
        private readonly ITeamAppService<TEnumTeamTypeId> teamAppService;

        /// <summary>
        /// The role application service.
        /// </summary>
        private readonly IRoleAppService roleAppService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAppService{TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId}" /> class.
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
        public AuthAppService(
#if BIA_FRONT_FEATURE
            IBaseUserAppService<TUserDto, TUser> userAppService,
            ITeamAppService<TEnumTeamTypeId> teamAppService,
            IRoleAppService roleAppService,
            IIdentityProviderRepository identityProviderRepository,
#endif
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            ILogger<AuthAppService<TUserDto, TUser, TEnumRoleId, TEnumTeamTypeId>> logger,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ILdapRepositoryHelper ldapRepositoryHelper)
        {
#if BIA_FRONT_FEATURE
            this.userAppService = userAppService;
            this.teamAppService = teamAppService;
            this.roleAppService = roleAppService;
            this.identityProviderRepository = identityProviderRepository;
            this.rolesConfiguration = biaNetconfiguration.Value.Roles;
#endif
            this.jwtFactory = jwtFactory;
            this.claimsPrincipal = principal as BiaClaimsPrincipal;
            this.userPermissionDomainService = userPermissionDomainService;
            this.logger = logger;
            this.userDirectoryHelper = userDirectoryHelper;
            this.ldapDomains = biaNetconfiguration.Value.Authentication.LdapDomains;
            this.ldapRepositoryHelper = ldapRepositoryHelper;
        }
#if BIA_BACK_TO_BACK_AUTH

        /// <inheritdoc cref="IAuthAppService.LoginAsync"/>
        public async Task<string> LoginAsync()
        {
            // Check if current user is authenticated
            this.CheckIsAuthenticated();

            // Get informations in Claims
            string sid = this.GetSid();
            string login = this.GetLogin();
            string domain = this.GetDomain();

            // Get Global Roles
            List<string> globalRoles = await this.GetGlobalRolesAsync(sid: sid, domain: domain);

            // Get Permissions
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles);

            // Check User Permissions
            this.CheckUserPermissions(userPermissions);

            // Sort User Permissions
            userPermissions.Sort();

            // Create Token Dto
            TokenDto<UserDataDto> tokenDto = new TokenDto<UserDataDto>()
            {
                Login = login,
                RoleIds = new List<int>(),
                Permissions = userPermissions,
                UserData = new UserDataDto(),
            };

            // Create AuthInfo
            AuthInfoDto<AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, default(AdditionalInfoDto), new LoginParamDto());

            return authInfo?.Token;
        }
#endif
#if BIA_FRONT_FEATURE

        /// <inheritdoc cref="IAuthAppService.LoginOnTeamsAsync"/>
        public async Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam, ImmutableList<BiaTeamConfig<Team>> teamsConfig)
        {
            // Check if current user is authenticated
            this.CheckIsAuthenticated();

            AuthInfoDto<AdditionalInfoDto> authInfo = await this.GetLoginToken(loginParam, true, teamsConfig);

            if (!string.IsNullOrWhiteSpace(loginParam.BaseUserLogin) && JwtFactory.HasRole(authInfo.Token, BiaRights.Impersonation.ConnectionRights))
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
        private static List<int> GetRoleIds(List<string> roles)
        {
            List<int> roleIds = new List<int>();
            foreach (string role in roles)
            {
                if (Enum.TryParse<TEnumRoleId>(role, out var roleId) && !roleIds.Contains(Convert.ToInt32(roleId)))
                {
                    roleIds.Add(Convert.ToInt32(roleId));
                }
            }

            return roleIds;
        }

        private async Task<AuthInfoDto<AdditionalInfoDto>> GetLoginToken(LoginParamDto loginParam, bool withCredentials, ImmutableList<BiaTeamConfig<Team>> teamsConfig)
        {
            // Get informations in Claims
            string sid = this.GetSid();
            string login = withCredentials ? this.GetLogin() : loginParam.BaseUserLogin;
            string domain = this.GetDomain();
            string identityKey = withCredentials ? this.GetIdentityKey() : loginParam.BaseUserLogin;

            // Get UserInfo
            UserInfoDto userInfo = await this.GetUserInfo(loginParam, login, identityKey);

            // Get Global Roles
            List<string> globalRoles = await this.GetGlobalRolesAsync(sid: sid, domain: domain, userInfo: userInfo, withCredentials);

            // Fill UserInfo
            userInfo = await this.CreateOrUpdateUserInDatabase(sid, identityKey, userInfo, globalRoles);
            this.userAppService.SelectDefaultLanguage(userInfo);

            // Get User AppRoot Roles
            if (userInfo?.Id > 0 && globalRoles.Contains(Crosscutting.Common.Constants.Role.User))
            {
                IEnumerable<string> userAppRootRoles = await this.roleAppService.GetUserRolesAsync(userInfo.Id);
                globalRoles.AddRange(userAppRootRoles);
            }

            List<int> roleIds = GetRoleIds(globalRoles);

            // Get Permissions
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles, loginParam.LightToken);

            IEnumerable<BaseDtoVersionedTeam> allTeams = new List<BaseDtoVersionedTeam>();
            UserDataDto userData = new UserDataDto();

            // Get Fine Grained Permissions
            if (loginParam.FineGrainedPermission && userInfo?.Id > 0)
            {
                // Get All Teams
                allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userPermissions);

                // Get Fine Grained Roles
                List<string> fineGrainedRoles = await this.GetFineRolesAsync(loginParam, userData, userInfo, allTeams, teamsConfig);
                List<int> fineGrainedRoleIds = GetRoleIds(fineGrainedRoles);
                roleIds = roleIds.Union(fineGrainedRoleIds).ToList();

                // Translate Roles in Permissions
                List<string> fineGrainedUserPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(fineGrainedRoles, loginParam.LightToken);

                // Concat global permissions and fine grained permissions
                userPermissions = userPermissions.Union(fineGrainedUserPermissions).ToList();
            }

            // Check User Permissions
            this.CheckUserPermissions(userPermissions);

            // Sort User Permissions
            userPermissions.Sort();

            // Create Token Dto
            TokenDto<UserDataDto> tokenDto = new TokenDto<UserDataDto>()
            {
                Login = login,
                Id = (userInfo?.Id).GetValueOrDefault(),
                RoleIds = roleIds,
                Permissions = userPermissions,
                UserData = userData,
            };

            // Get AdditionalInfoDto
            AdditionalInfoDto additionalInfo = this.GetAdditionalInfo(loginParam, userInfo, allTeams, userData);

            // Create AuthInfo
            AuthInfoDto<AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionalInfo, loginParam);

            return authInfo;
        }
#endif

        /// <summary>
        /// Checks the user permissions.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <exception cref="UnauthorizedException">No permission found.</exception>
        private void CheckUserPermissions(List<string> userPermissions)
        {
            if (!userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
            }
        }

        /// <summary>
        /// Checks if user is Authenticated.
        /// </summary>
        /// <param name="claimsPrincipal">The identity.</param>
        private void CheckIsAuthenticated()
        {
            if (this.claimsPrincipal.Identity?.IsAuthenticated != true)
            {
                if (this.claimsPrincipal.Identity == null)
                {
                    this.logger.LogInformation("Unauthorized because identity is null");
                }
                else
                {
                    this.logger.LogInformation("Unauthorized because not authenticated");
                }

                throw new UnauthorizedException();
            }
        }

        /// <summary>
        /// Gets the global roles.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="userInfo">The user information.</param>
        /// <returns>Global roles.</returns>
        /// <exception cref="UnauthorizedException">No roles found.</exception>
        private async Task<List<string>> GetGlobalRolesAsync(string sid, string domain, UserInfoDto userInfo = default, bool withCredentials = true)
        {
            List<string> globalRoles = await this.userDirectoryHelper.GetUserRolesAsync(claimsPrincipal: this.claimsPrincipal, userInfoDto: userInfo, sid: sid, domain: domain, withCredentials: withCredentials);

            // If the user has no role
            if (globalRoles?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                throw new UnauthorizedException("No roles found");
            }

            return globalRoles;
        }

        /// <summary>
        /// Gets the sid.
        /// </summary>
        /// <returns>The sid.</returns>
        private string GetSid()
        {
            return this.claimsPrincipal.GetClaimValue(ClaimTypes.PrimarySid);
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        /// <returns>The login.</returns>
        private string GetLogin()
        {
            var login = this.claimsPrincipal.GetUserLogin()?.Split('\\').LastOrDefault()?.ToUpper();
            if (string.IsNullOrEmpty(login))
            {
                this.logger.LogWarning("Unauthorized because bad login");
                throw new BadRequestException("Incorrect login");
            }

            return login;
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <returns>The domain.</returns>
        private string GetDomain()
        {
            string domain = null;

            if (this.claimsPrincipal.Identity.Name?.Contains('\\') == true)
            {
                domain = this.claimsPrincipal.Identity.Name.Split('\\').FirstOrDefault();
                if (
                        !this.ldapDomains.Any(ld => ld.Name.Equals(domain))
                        &&
                        !(
                            this.ldapDomains.Any(ld => this.ldapRepositoryHelper.IsLocalMachineName(ld.Name, true))
                            &&
                            this.ldapRepositoryHelper.IsLocalMachineName(domain, false))
                        &&
                        !(
                            this.ldapDomains.Any(ld => this.ldapRepositoryHelper.IsServerDomain(ld.Name, true))
                            &&
                            this.ldapRepositoryHelper.IsServerDomain(domain, false)))
                {
                    this.logger.LogInformation("Unauthorized because bad domain");
                    throw new UnauthorizedException();
                }
            }

            return domain;
        }
#if BIA_FRONT_FEATURE

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="login">The login.</param>
        /// <param name="identityKey">The identity key.</param>
        /// <returns>A UserInfo Dto.</returns>
        private async Task<UserInfoDto> GetUserInfo(LoginParamDto loginParam, string login, string identityKey)
        {
            // Get userInfo if needed (it requires an user in database)
            UserInfoDto userInfo = null;

            if (loginParam.FineGrainedPermission || loginParam.AdditionalInfos || this.UseUserRole())
            {
                userInfo = await this.userAppService.GetUserInfoAsync(identityKey);
            }

            // If the user does not exist in the database
            // We create a UserInfoDto object from principal
            userInfo ??= new UserInfoDto
            {
                Login = login,
                FirstName = this.claimsPrincipal.GetClaimValue(ClaimTypes.GivenName),
                LastName = this.claimsPrincipal.GetClaimValue(ClaimTypes.Surname),
                Country = this.claimsPrincipal.GetClaimValue(ClaimTypes.Country),
            };

            return userInfo;
        }

        /// <summary>
        /// Gets the identity key.
        /// </summary>
        /// <returns>The identity key.</returns>
        private string GetIdentityKey()
        {
            // If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
            return this.GetLogin();
        }

        /// <summary>
        /// Checks if the rolesConfiguration contains the 'User' role.
        /// </summary>
        /// <returns>Return true if the rolesConfiguration contains the 'User' role.</returns>
        private bool UseUserRole()
        {
            return this.rolesConfiguration != null && this.rolesConfiguration.Any(r =>
                r.Label == Crosscutting.Common.Constants.Role.User);
        }

        /// <summary>
        /// Gets the additional information.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="allTeams">All teams.</param>
        /// <param name="userData">The user data.</param>
        /// <returns>A AdditionalInfo Dto.</returns>
        private AdditionalInfoDto GetAdditionalInfo(LoginParamDto loginParam, UserInfoDto userInfo, IEnumerable<BaseDtoVersionedTeam> allTeams, UserDataDto userData)
        {
            AdditionalInfoDto additionalInfo = default;

            if (loginParam.AdditionalInfos)
            {
                var allTeamsFilteredByCurrentParent = allTeams.Where(t => TeamConfig.Config.Exists(tc => tc.TeamTypeId == t.TeamTypeId && (
                    tc.Parents == null
                    ||
                    tc.Parents.Exists(p => userData.CurrentTeams.Any(ct => ct.TeamId == t.ParentTeamId))))).ToList();

                additionalInfo = new AdditionalInfoDto
                {
                    UserInfo = userInfo,
                    Teams = allTeamsFilteredByCurrentParent.OrderBy(x => x.Title).ToList(),
                };
            }

            return additionalInfo;
        }

        /// <summary>
        /// Creates the or update user in database.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="identityKey">The identity key.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="globalRoles">The global roles.</param>
        /// <returns>A UserInfoDto.</returns>
        private async Task<UserInfoDto> CreateOrUpdateUserInDatabase(string sid, string identityKey, UserInfoDto userInfo, List<string> globalRoles)
        {
            if (globalRoles.Contains(Crosscutting.Common.Constants.Role.User))
            {
                if (!(userInfo?.Id > 0))
                {
                    // automatic creation from ldap, only use if user do not need fine Role on team.
                    try
                    {
                        UserFromDirectory userFromDirectory = null;

                        if (!string.IsNullOrWhiteSpace(sid))
                        {
                            userFromDirectory = await this.userDirectoryHelper.ResolveUserByIdentityKey(identityKey);
                        }
                        else
                        {
                            userFromDirectory = await this.identityProviderRepository.FindUserAsync(identityKey);
                        }

                        if (userFromDirectory != null)
                        {
                            TUser user = await this.userAppService.AddUserFromUserDirectoryAsync(identityKey, userFromDirectory);
                            userInfo = this.userAppService.CreateUserInfo(user);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Cannot create user... Probably database is read only...");
                    }
                }
                else
                {
                    try
                    {
                        // The date of the last connection is updated in the database
                        await this.userAppService.UpdateLastLoginDateAndActivate(userInfo.Id, true);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Cannot update last login date... Probably database is read only...");
                    }
                }
            }

            return userInfo;
        }

        /// <summary>
        /// Gets the roles asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="allTeams">All teams.</param>
        /// <returns>List of role.</returns>
        private async Task<List<string>> GetFineRolesAsync(LoginParamDto loginParam, UserDataDto userData, UserInfoDto userInfo, IEnumerable<BaseDtoVersionedTeam> allTeams, ImmutableList<BiaTeamConfig<Team>> teamsConfig)
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

                    IEnumerable<RoleDto> roles = await this.roleAppService.GetMemberRolesAsync(currentTeam.TeamId, userInfo.Id);
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
                    allRoles.Add(teamConfig.RightPrefix + Crosscutting.Common.Constants.Role.TeamMemberSuffix);
                    allRoles.Add(teamConfig.RightPrefix + Crosscutting.Common.Constants.Role.TeamMemberOfOneSuffix);

                    // add computed roles (can be customized)
                }

                foreach (var teamConfig in teamsConfig)
                {
                    if (teamConfig.Parents == null && allTeams.Any(t => t.TeamTypeId == teamConfig.TeamTypeId))
                    {
                        allRoles.Add(teamConfig.RightPrefix + Crosscutting.Common.Constants.Role.TeamMemberOfOneSuffix);
                    }
                }
            }

            return allRoles;
        }
#endif
    }
}
// <copyright file="AuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    public class AuthAppService : IAuthAppService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthAppService> logger;

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

#if BIA_FRONT_FEATURE
        /// <summary>
        /// The role section in the BiaNet configuration.
        /// </summary>
        private readonly IEnumerable<BIA.Net.Core.Common.Configuration.Role> rolesConfiguration;

        /// <summary>
        /// The identity provider repository.
        /// </summary>
        private readonly Domain.RepoContract.IIdentityProviderRepository identityProviderRepository;

        /// <summary>
        /// The user application service.
        /// </summary>
        private readonly IUserAppService userAppService;

        /// <summary>
        /// The team application service.
        /// </summary>
        private readonly ITeamAppService teamAppService;

        /// <summary>
        /// The role application service.
        /// </summary>
        private readonly IRoleAppService roleAppService;

        /// <summary>
        /// The user profile repository.
        /// </summary>
        private readonly Domain.RepoContract.IUserProfileRepository userProfileRepository;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAppService"/> class.
        /// </summary>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="userProfileRepository">The user profile repository.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userPermissionDomainService">The user permission domain service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="biaNetconfiguration">The bia netconfiguration.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        public AuthAppService(
#if BIA_FRONT_FEATURE
            IUserAppService userAppService,
            ITeamAppService teamAppService,
            IRoleAppService roleAppService,
            Domain.RepoContract.IUserProfileRepository userProfileRepository,
            Domain.RepoContract.IIdentityProviderRepository identityProviderRepository,
#endif
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            ILogger<AuthAppService> logger,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper)
        {
#if BIA_FRONT_FEATURE
            this.userAppService = userAppService;
            this.teamAppService = teamAppService;
            this.roleAppService = roleAppService;
            this.userProfileRepository = userProfileRepository;
            this.identityProviderRepository = identityProviderRepository;
            this.rolesConfiguration = biaNetconfiguration.Value.Roles;
#endif
            this.jwtFactory = jwtFactory;
            this.claimsPrincipal = principal as BiaClaimsPrincipal;
            this.userPermissionDomainService = userPermissionDomainService;
            this.logger = logger;
            this.userDirectoryHelper = userDirectoryHelper;
            this.ldapDomains = biaNetconfiguration.Value.Authentication.LdapDomains;
        }

#if BIA_SERVICE_API
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
        public async Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam)
        {
            // Check if current user is authenticated
            this.CheckIsAuthenticated();

            // Get informations in Claims
            string sid = this.GetSid();
            string login = this.GetLogin();
            string domain = this.GetDomain();
            string identityKey = this.GetIdentityKey();

            // Get UserInfo && UserProfile
            Task<UserProfileDto> userProfileTask = this.GetUserProfileTask(loginParam, identityKey);
            UserInfoDto userInfo = await this.GetUserInfo(loginParam, login, identityKey);

            // Get Global Roles
            List<string> globalRoles = await this.GetGlobalRolesAsync(sid: sid, domain: domain, userInfo: userInfo);
            List<int> roleIds = this.GetRoleIds(globalRoles);

            // Fill UserInfo
            userInfo = await this.CreateOrUpdateUserInDatabase(sid, identityKey, userInfo, globalRoles);
            this.userAppService.SelectDefaultLanguage(userInfo);

            // Get User AppRoot Roles
            if (userInfo?.Id > 0 && globalRoles.Contains(Crosscutting.Common.Constants.Role.User))
            {
                IEnumerable<string> userAppRootRoles = await this.roleAppService.GetUserRolesAsync(userInfo.Id);
                globalRoles.AddRange(userAppRootRoles);
            }

            // Get Permissions
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles, loginParam.LightToken);

            IEnumerable<TeamDto> allTeams = new List<TeamDto>();
            UserDataDto userData = new UserDataDto();

            // Get Fine Grained Permissions
            if (loginParam.FineGrainedPermission && userInfo?.Id > 0)
            {
                // Get All Teams
                allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userPermissions);

                // Get Fine Grained Roles
                List<string> fineGrainedRoles = await this.GetFineRolesAsync(loginParam, userData, userInfo, allTeams);
                List<int> fineGrainedRoleIds = this.GetRoleIds(fineGrainedRoles);
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

            // Get UserProfileDto && AdditionalInfoDto
            UserProfileDto userProfile = await this.GetUserProfile(userProfileTask);
            AdditionalInfoDto additionalInfo = this.GetAdditionalInfo(loginParam, userInfo, allTeams, userData, userProfile);

            // Create AuthInfo
            AuthInfoDto<AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionalInfo, loginParam);

            return authInfo;
        }
#endif

        /// <summary>
        /// Checks the user permissions.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <exception cref="BIA.Net.Core.Common.Exceptions.UnauthorizedException">No permission found.</exception>
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
        /// <exception cref="BIA.Net.Core.Common.Exceptions.UnauthorizedException">No roles found.</exception>
        private async Task<List<string>> GetGlobalRolesAsync(string sid, string domain, UserInfoDto userInfo = default)
        {
            List<string> globalRoles = await this.userDirectoryHelper.GetUserRolesAsync(claimsPrincipal: this.claimsPrincipal, userInfoDto: userInfo, sid: sid, domain: domain);

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
                if (!this.ldapDomains.Any(ld => ld.Name.Equals(domain)))
                {
                    this.logger.LogInformation("Unauthorized because bad domain");
                    throw new UnauthorizedException();
                }
            }

            return domain;
        }

#if BIA_FRONT_FEATURE

        /// <summary>
        /// Gets the role ids.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Role ids.</returns>
        private List<int> GetRoleIds(List<string> roles)
        {
            List<int> roleIds = new List<int>();
            foreach (string role in roles)
            {
                if (Enum.TryParse<RoleId>(role, out var roleId) && !roleIds.Contains((int)roleId))
                {
                    roleIds.Add((int)roleId);
                }
            }

            return roleIds;
        }

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
        /// Check if UserInDb is requiered.
        /// </summary>
        /// <returns>True if user in db is in configuration file.</returns>
        private bool UseUserRole()
        {
            return this.rolesConfiguration != null && this.rolesConfiguration.Any(r =>
                r.Label == Crosscutting.Common.Constants.Role.User);
        }

        private Task<UserProfileDto> GetUserProfileTask(LoginParamDto loginParam, string identityKey)
        {
            // Get user profil async
            Task<UserProfileDto> userProfileTask = null;
            if (loginParam.AdditionalInfos)
            {
                userProfileTask = this.userProfileRepository.GetAsync(identityKey);
            }

            return userProfileTask;
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="userProfileTask">The user profile task.</param>
        /// <returns>A UserProfile Dto.</returns>
        private async Task<UserProfileDto> GetUserProfile(Task<UserProfileDto> userProfileTask)
        {
            UserProfileDto userProfile = null;
            if (userProfileTask != null)
            {
                userProfile = await userProfileTask;
                userProfile ??= new UserProfileDto { Theme = Crosscutting.Common.Constants.DefaultValues.Theme };
            }

            return userProfile;
        }

        /// <summary>
        /// Gets the additional information.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="allTeams">All teams.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>A AdditionalInfo Dto.</returns>
        private AdditionalInfoDto GetAdditionalInfo(LoginParamDto loginParam, UserInfoDto userInfo, IEnumerable<TeamDto> allTeams, UserDataDto userData, UserProfileDto userProfile)
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
                    UserProfile = userProfile,
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
                            User user = await this.userAppService.AddUserFromUserDirectoryAsync(identityKey, userFromDirectory);
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
        private async Task<List<string>> GetFineRolesAsync(LoginParamDto loginParam, UserDataDto userData, UserInfoDto userInfo, IEnumerable<TeamDto> allTeams)
        {
            // the main roles
            List<string> allRoles = new List<string>();

            // get user rights
            if (loginParam.TeamsConfig != null)
            {
                foreach (TeamConfigDto loginTeamConfig in loginParam.TeamsConfig)
                {
                    CurrentTeamDto teamLogin = loginParam.CurrentTeamLogins != null ? Array.Find(loginParam.CurrentTeamLogins, ct => ct.TeamTypeId == loginTeamConfig.TeamTypeId) : null;
                    if (teamLogin == null && loginTeamConfig.InHeader)
                    {
                        // if it is in header we select the default one with default roles.
                        teamLogin = new CurrentTeamDto
                        {
                            TeamTypeId = loginTeamConfig.TeamTypeId,
                            TeamId = 0,
                            UseDefaultRoles = true,
                            CurrentRoleIds = { },
                        };
                    }

                    if (teamLogin != null)
                    {
                        IEnumerable<TeamDto> teams = allTeams.Where(t => t.TeamTypeId == teamLogin.TeamTypeId);
                        TeamDto team = teams.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                        CurrentTeamDto currentTeam = new CurrentTeamDto()
                        {
                            TeamTypeId = teamLogin.TeamTypeId,
                        };

                        if (team != null)
                        {
                            if (teamLogin.TeamId > 0 && teams.Any(s => s.Id == teamLogin.TeamId))
                            {
                                currentTeam.TeamId = teamLogin.TeamId;
                                currentTeam.TeamTitle = teams.First(s => s.Id == teamLogin.TeamId).Title;
                            }
                            else
                            {
                                currentTeam.TeamId = team.Id;
                                currentTeam.TeamTitle = team.Title;
                            }
                        }

                        if (currentTeam.TeamId > 0)
                        {
                            IEnumerable<RoleDto> roles = await this.roleAppService.GetMemberRolesAsync(currentTeam.TeamId, userInfo.Id);
                            BIA.Net.Core.Common.Enum.RoleMode roleMode = Array.Find(loginParam.TeamsConfig, r => r.TeamTypeId == currentTeam.TeamTypeId)?.RoleMode ?? BIA.Net.Core.Common.Enum.RoleMode.AllRoles;

                            if (roleMode == BIA.Net.Core.Common.Enum.RoleMode.AllRoles)
                            {
                                currentTeam.CurrentRoleIds = roles.Select(r => r.Id).ToList();
                            }
                            else if (roleMode == BIA.Net.Core.Common.Enum.RoleMode.SingleRole)
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

                            userData.CurrentTeams.Add(currentTeam);

                            // add the sites roles (filter if singleRole mode is used)
                            allRoles.AddRange(roles.Where(r => currentTeam.CurrentRoleIds.Exists(id => id == r.Id)).Select(r => r.Code).ToList());

                            foreach (var teamConfig in TeamConfig.Config)
                            {
                                if (currentTeam.TeamTypeId == teamConfig.TeamTypeId)
                                {
                                    allRoles.Add(teamConfig.RightPrefix + Crosscutting.Common.Constants.Role.TeamMemberSuffix);
                                }

                                if (teamConfig.Parents != null && allTeams.Any(t => t.TeamTypeId == teamConfig.TeamTypeId && t.ParentTeamId == currentTeam.TeamId))
                                {
                                    allRoles.Add(teamConfig.RightPrefix + Crosscutting.Common.Constants.Role.TeamMemberOfOneSuffix);
                                }
                            }

                            // add computed roles (can be customized)
                        }
                    }
                }

                foreach (var teamConfig in TeamConfig.Config)
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

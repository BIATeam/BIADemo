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
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Application.User.IAuthAppService" />
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
        private readonly IUserProfileRepository userProfileRepository;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The domain section in the BiaNet configuration.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// The role section in the BiaNet configuration.
        /// </summary>
        private readonly IEnumerable<BIA.Net.Core.Common.Configuration.Role> rolesConfiguration;

        /// <summary>
        /// The identity provider repository.
        /// </summary>
        private readonly IIdentityProviderRepository identityProviderRepository;

        /// <summary>
        /// return true if use database.
        /// </summary>
        private readonly bool isDatabaseUsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAppService" /> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userPermissionDomainService">The user permission domain service.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="userProfileRepository">The user profile repository.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="biaNetconfiguration">The BiaNetSection configuration.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        public AuthAppService(
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            IUserAppService userAppService,
            ITeamAppService teamAppService,
            ILogger<AuthAppService> logger,
            IRoleAppService roleAppService,
            IUserProfileRepository userProfileRepository,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            IIdentityProviderRepository identityProviderRepository)
        {
            this.jwtFactory = jwtFactory;
            this.claimsPrincipal = principal as BiaClaimsPrincipal;
            this.userPermissionDomainService = userPermissionDomainService;
            this.userAppService = userAppService;
            this.teamAppService = teamAppService;
            this.logger = logger;
            this.roleAppService = roleAppService;
            this.userProfileRepository = userProfileRepository;
            this.userDirectoryHelper = userDirectoryHelper;
            this.ldapDomains = biaNetconfiguration.Value.Authentication.LdapDomains;
            this.rolesConfiguration = biaNetconfiguration.Value.Roles;
            this.identityProviderRepository = identityProviderRepository;
            this.isDatabaseUsed = !string.IsNullOrWhiteSpace(configuration.GetConnectionString("BIADemoDatabase"));
        }

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>
        /// AuthInfo.
        /// </returns>
        public async Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam)
        {
            this.CheckIsAuthenticated();

            string sid = this.GetSid();
            string login = this.GetLogin();
            string domain = this.GetDomain();
            string identityKey = this.GetIdentityKey();

            Task<UserProfileDto> userProfileTask = this.GetUserProfileTask(loginParam, identityKey);

            UserInfoDto userInfo = await this.GetUserInfo(loginParam, login, identityKey);

            List<string> globalRoles = await this.GetGlobalRolesAsync(sid: sid, domain: domain, userInfo: userInfo);
            List<int> roleIds = this.GetRoleIds(globalRoles);

            IEnumerable<TeamDto> allTeams = new List<TeamDto>();
            UserDataDto userData = new UserDataDto();
            List<string> userPermissions = null;

            if (this.isDatabaseUsed)
            {
                userInfo = await this.CreateOrUpdateUserInDatabase(sid, identityKey, userInfo, globalRoles);

                if (userInfo?.Id > 0 && globalRoles.Contains(Constants.Role.User))
                {
                    IEnumerable<string> userAppRootRoles = await this.roleAppService.GetUserRolesAsync(userInfo.Id);
                    globalRoles.AddRange(userAppRootRoles);
                }

                userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles, loginParam.LightToken);

                // Compute fine grained user permissions if needed (it requires an user in database)
                if (loginParam.FineGrainedPermission && userInfo?.Id > 0)
                {
                    allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userPermissions);
                    List<string> fineGrainedRoles = await this.GetFineRolesAsync(loginParam, userData, userInfo, allTeams);

                    List<int> fineGrainedRoleIds = this.GetRoleIds(fineGrainedRoles);
                    roleIds = roleIds.Union(fineGrainedRoleIds).ToList();

                    // translate roles in permission
                    List<string> fineGrainedUserPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(fineGrainedRoles, loginParam.LightToken);
                    userPermissions = userPermissions.Union(fineGrainedUserPermissions).ToList();
                }
            }
            else
            {
                userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles, loginParam.LightToken);
            }

            this.CheckUserPermissions(userPermissions);

            this.userAppService.SelectDefaultLanguage(userInfo);

            userPermissions.Sort();
            roleIds.Sort();

            TokenDto<UserDataDto> tokenDto = new TokenDto<UserDataDto>()
            {
                Login = login,
                Id = (userInfo?.Id).GetValueOrDefault(),
                RoleIds = roleIds,
                Permissions = userPermissions,
                UserData = userData,
            };

            UserProfileDto userProfile = await this.GetUserProfile(userProfileTask);
            AdditionalInfoDto additionalInfo = this.GetAdditionalInfo(loginParam, userInfo, allTeams, userData, userProfile);

            AuthInfoDto<AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionalInfo, loginParam);

            return authInfo;
        }

        private void CheckUserPermissions(List<string> userPermissions)
        {
            if (!userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
            }
        }

        private AdditionalInfoDto GetAdditionalInfo(LoginParamDto loginParam, UserInfoDto userInfo, IEnumerable<TeamDto> allTeams, UserDataDto userData, UserProfileDto userProfile)
        {
            AdditionalInfoDto additionalInfo = null;
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

        private async Task<UserProfileDto> GetUserProfile(Task<UserProfileDto> userProfileTask)
        {
            UserProfileDto userProfile = null;
            if (userProfileTask != null)
            {
                userProfile = await userProfileTask;
                userProfile ??= new UserProfileDto { Theme = Constants.DefaultValues.Theme };
            }

            return userProfile;
        }

        private async Task<UserInfoDto> CreateOrUpdateUserInDatabase(string sid, string identityKey, UserInfoDto userInfo, List<string> globalRoles)
        {
            if (this.isDatabaseUsed && globalRoles.Contains(Constants.Role.User))
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

        private async Task<UserInfoDto> GetUserInfo(LoginParamDto loginParam, string login, string identityKey)
        {
            // Get userInfo if needed (it requires an user in database)
            UserInfoDto userInfo = null;
            if (this.isDatabaseUsed && (loginParam.FineGrainedPermission || loginParam.AdditionalInfos || this.UseUserRole()))
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
        /// Check if UserInDb is requiered.
        /// </summary>
        /// <returns>True if user in db is in configuration file.</returns>
        private bool UseUserRole()
        {
            return this.rolesConfiguration != null && this.rolesConfiguration.Any(r =>
                r.Label == Constants.Role.User);
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

        private async Task<List<string>> GetGlobalRolesAsync(string sid, string domain, UserInfoDto userInfo)
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

        private string GetIdentityKey()
        {
            // If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
            return this.GetLogin();
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

                            userData.CurrentTeams.Add(currentTeam);

                            // add the sites roles (filter if singleRole mode is used)
                            allRoles.AddRange(roles.Where(r => currentTeam.CurrentRoleIds.Exists(id => id == r.Id)).Select(r => r.Code).ToList());

                            foreach (var teamConfig in TeamConfig.Config)
                            {
                                if (currentTeam.TeamTypeId == teamConfig.TeamTypeId)
                                {
                                    allRoles.Add(teamConfig.RightPrefix + Constants.Role.TeamMemberSuffix);
                                }

                                if (teamConfig.Parents != null && allTeams.Any(t => t.TeamTypeId == teamConfig.TeamTypeId && t.ParentTeamId == currentTeam.TeamId))
                                {
                                    allRoles.Add(teamConfig.RightPrefix + Constants.Role.TeamMemberOfOneSuffix);
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
                        allRoles.Add(teamConfig.RightPrefix + Constants.Role.TeamMemberOfOneSuffix);
                    }
                }
            }

            return allRoles;
        }
    }
}

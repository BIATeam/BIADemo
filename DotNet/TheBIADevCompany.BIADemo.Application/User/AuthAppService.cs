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
        private readonly BIAClaimsPrincipal claimsPrincipal;

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
        /// The connectionString.
        /// </summary>
        private readonly string connectionString;

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
            this.claimsPrincipal = principal as BIAClaimsPrincipal;
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
            this.connectionString = configuration.GetConnectionString("BIADemoDatabase");
        }

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>
        /// AuthInfo.
        /// </returns>
        public async Task<AuthInfoDto<UserDataDto, AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam)
        {
            // Check inputs parameter
            this.CheckIsAuthenticated();
            this.GetIdentityKey(out string identityKey, out string sid, out string login);

            // Get user profil async
            Task<UserProfileDto> userProfileTask = null;
            if (loginParam.AdditionalInfos)
            {
                userProfileTask = this.userProfileRepository.GetAsync(identityKey);
            }

            // Get userInfo if needed (it requires an user in database)
            UserInfoDto userInfo = null;
            if (this.connectionString != null && (loginParam.FineGrainedPermission || loginParam.AdditionalInfos || this.UseUserRole()))
            {
                userInfo = await this.userAppService.GetUserInfoAsync(identityKey);
            }

            // Get domain
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

            // Get global roles
            List<string> globalRoles = await this.userDirectoryHelper.GetUserRolesAsync(claimsPrincipal: this.claimsPrincipal, userInfoDto: userInfo, sid: sid, domain: domain);

            // If the user has no role
            if (globalRoles == null || globalRoles?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                throw new UnauthorizedException("No roles found");
            }

            if (globalRoles.Contains(Constants.Role.User))
            {
                if (userInfo == null)
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

                        User user = await this.userAppService.AddUserFromUserDirectoryAsync(identityKey, userFromDirectory);
                        userInfo = this.userAppService.CreateUserInfo(user);
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

            if (userInfo != null && globalRoles.Contains(Constants.Role.User))
            {
                IEnumerable<string> userAppRootRoles = await this.roleAppService.GetUserRolesAsync(userInfo.Id);
                globalRoles.AddRange(userAppRootRoles);
            }

            if (globalRoles == null || !globalRoles.Any())
            {
                this.logger.LogInformation("Unauthorized because no role found");
                throw new UnauthorizedException("No role found");
            }

            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles, loginParam.LightToken);
            IEnumerable<TeamDto> allTeams = new List<TeamDto>();
            UserDataDto userData = new ();

            // Compute fine grained user permissions if needed (it requires an user in database)
            if (loginParam.FineGrainedPermission && userInfo != null)
            {
                allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userPermissions);
                List<string> fineGrainedRoles = await this.GetFineRolesAsync(loginParam, userData, userInfo, allTeams);

                // translate roles in permission
                List<string> fineGrainedUserPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(fineGrainedRoles, loginParam.LightToken);
                userPermissions = userPermissions.Union(fineGrainedUserPermissions).ToList();
            }

            if (!userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
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

            this.userAppService.SelectDefaultLanguage(userInfo);

            TokenDto<UserDataDto> tokenDto = new () { Login = login, Id = userInfo.Id, Permissions = userPermissions, UserData = userData };

            UserProfileDto userProfile = null;
            if (userProfileTask != null)
            {
                userProfile = await userProfileTask;
                userProfile ??= new UserProfileDto { Theme = Constants.DefaultValues.Theme };
            }

            AdditionalInfoDto additionnalInfo = null;
            if (loginParam.AdditionalInfos)
            {
                additionnalInfo = new AdditionalInfoDto { UserInfo = userInfo, UserProfile = userProfile, Teams = allTeams.ToList() };
            }

            AuthInfoDto<UserDataDto, AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionnalInfo, loginParam);

            return authInfo;
        }

        /// <summary>
        /// Check if UserInDb is requiered.
        /// </summary>
        /// <returns>True if user in db is in configuration file.</returns>
        public bool UseUserRole()
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
                else if (!this.claimsPrincipal.Identity.IsAuthenticated)
                {
                    this.logger.LogInformation("Unauthorized because not authenticated");
                }

                throw new UnauthorizedException();
            }
        }

        /// <summary>
        /// Checks the input parameters.
        /// </summary>
        /// <param name="identityKey">The identityKey.</param>
        /// <param name="sid">The sid.</param>
        /// <param name="login">The login.</param>
        private void GetIdentityKey(out string identityKey, out string sid, out string login)
        {
            sid = this.GetSid();
            login = this.GetLogin();

            // If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
            identityKey = login;
        }

        private string GetSid()
        {
            return this.claimsPrincipal.GetClaimValue(ClaimTypes.PrimarySid);
        }

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
                foreach (TeamConfigDto teamConfig in loginParam.TeamsConfig)
                {
                    CurrentTeamDto teamLogin = loginParam.CurrentTeamLogins != null ? Array.Find(loginParam.CurrentTeamLogins, ct => ct.TeamTypeId == teamConfig.TeamTypeId) : null;
                    if (teamLogin == null && teamConfig.InHeader)
                    {
                        // if it is in header we select the default one with default roles.
                        teamLogin = new CurrentTeamDto
                        {
                            TeamTypeId = teamConfig.TeamTypeId,
                            TeamId = 0,
                            UseDefaultRoles = true,
                            CurrentRoleIds = { },
                        };
                    }

                    if (teamLogin != null)
                    {
                        IEnumerable<TeamDto> teams = allTeams.Where(t => t.TeamTypeId == teamLogin.TeamTypeId);
                        TeamDto team = teams.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                        CurrentTeamDto currentTeam = new ()
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

                            // add computed roles (can be customized)
                            if (currentTeam.TeamTypeId == (int)TeamTypeId.Site)
                            {
                                allRoles.Add(Constants.Role.SiteMember);
                            }

                            // Begin BIADemo
                            if (currentTeam.TeamTypeId == (int)TeamTypeId.AircraftMaintenanceCompany)
                            {
                                allRoles.Add(Constants.Role.AircraftMaintenanceCompanyMember);
                            }

                            if (currentTeam.TeamTypeId == (int)TeamTypeId.MaintenanceTeam)
                            {
                                allRoles.Add(Constants.Role.MaintenanceTeamMember);
                            }

                            // End BIADemo
                        }
                    }
                }
            }

            return allRoles;
        }
    }
}

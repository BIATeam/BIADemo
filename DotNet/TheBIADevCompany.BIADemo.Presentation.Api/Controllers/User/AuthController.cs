// <copyright file="AuthController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// The API controller used to authenticate users.
    /// </summary>
    public class AuthController : BiaControllerBaseNoToken
    {
        /// <summary>
        /// The JWT factory.
        /// </summary>
        private readonly IJwtFactory jwtFactory;

        /// <summary>
        /// The user application service.
        /// </summary>
        private readonly IUserAppService userAppService;

        /// <summary>
        /// The site application service.
        /// </summary>
        private readonly ITeamAppService teamAppService;

        /// <summary>
        /// The role application service.
        /// </summary>
        private readonly IRoleAppService roleAppService;

        /// <summary>
        /// The User Right domain service.
        /// </summary>
        private readonly IUserPermissionDomainService userPermissionDomainService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthController> logger;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="userPermissionDomainService">The User Right domain service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public AuthController(
            IJwtFactory jwtFactory,
            IUserAppService userAppService,
            ITeamAppService teamAppService,
            IRoleAppService roleAppService,
            IUserPermissionDomainService userPermissionDomainService,
            ILogger<AuthController> logger,
            IOptions<BiaNetSection> configuration)
        {
            this.jwtFactory = jwtFactory;
            this.userAppService = userAppService;
            this.teamAppService = teamAppService;
            this.roleAppService = roleAppService;
            this.logger = logger;
            this.userPermissionDomainService = userPermissionDomainService;
            this.ldapDomains = configuration.Value.Authentication.LdapDomains;
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login()
        {
            // used only by swagger.
            LoginParamDto loginParam = new LoginParamDto
            {
                TeamsConfig = new TeamConfigDto[]
                {
                    // this config is requerierd to simulate default site with swagger.
                    // it should correspond to the Front config (allEnvironments.Teams)
                    new TeamConfigDto { TeamTypeId = (int)TeamTypeId.Site, RoleMode = RoleMode.AllRoles, InHeader = true },

                    // Begin BIADemo
                    new TeamConfigDto() { TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany, RoleMode = RoleMode.MultiRoles, InHeader = true },
                    new TeamConfigDto() { TeamTypeId = (int)TeamTypeId.MaintenanceTeam, RoleMode = RoleMode.AllRoles, InHeader = false },

                    // End BIADemo
                },
                CurrentTeamLogins = null,
                LightToken = false,
            };

            return await this.LoginOnTeams(loginParam);
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="loginParam">The parameters for login.</param>
        /// <returns>
        /// The JWT if authenticated.
        /// </returns>
        [HttpPost("loginAndTeams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginOnTeams(LoginParamDto loginParam)
        {
            // user data
            var userData = new UserDataDto();

            var identity = this.User.Identity;
            if (identity == null || !identity.IsAuthenticated)
            {
                if (identity == null)
                {
                    this.logger.LogInformation("Unauthorized because identity is null");
                }
                else if (!identity.IsAuthenticated)
                {
                    this.logger.LogInformation("Unauthorized because not authenticated");
                }

                return this.Unauthorized();
            }

            var login = identity.Name.Split('\\').LastOrDefault();
#pragma warning disable CA1416 // Validate platform compatibility
            var sid = ((System.Security.Principal.WindowsIdentity)identity).User.Value;
#pragma warning restore CA1416 // Validate platform compatibility

            if (string.IsNullOrEmpty(login))
            {
                this.logger.LogWarning("Unauthorized because bad login");
                return this.Unauthorized("Incorrect login");
            }

            if (string.IsNullOrEmpty(sid))
            {
                this.logger.LogWarning("Unauthorized because bad sid");
                return this.Unauthorized("Incorrect sid");
            }

            var domain = identity.Name.Split('\\').FirstOrDefault();
            if (!this.ldapDomains.Any(ld => ld.Name.Equals(domain)))
            {
                this.logger.LogWarning("Unauthorized because bad domain");
                return this.Unauthorized("Incorrect domain");
            }

            // parallel launch the get user profile
            Task<UserProfileDto> userProfileTask = null;
            if (!loginParam.LightToken)
            {
                userProfileTask = this.userAppService.GetUserProfileAsync(login);
            }

            // Get userInfo
            UserInfoDto userInfo = await this.userAppService.GetUserInfoAsync(login);

            // get roles
            var userRoles = await this.userAppService.GetUserDirectoryRolesAsync(userInfo?.IsActive == true, sid, domain);

            // If the user has no role
            if (userRoles?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                return this.Unauthorized("No roles found");
            }

            if (userInfo == null && !string.IsNullOrWhiteSpace(sid) && userRoles.Contains(Constants.Role.User))
            {
                // automatic creation from ldap, only use if user do not need fine Role on team.
                try
                {
                    userInfo = await this.userAppService.CreateUserInfoFromLdapAsync(sid, login);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Cannot create user... Probably database is read only...");
                }
            }

            if (userInfo != null)
            {
                try
                {
                    // The date of the last connection is updated in the database
                    await this.userAppService.UpdateLastLoginDateAndActivate(userInfo.Id, userRoles.Contains(Constants.Role.User));
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Cannot update last login date... Probably database is read only...");
                }
            }

            // If the user does not exist in the database
            if (userInfo == null)
            {
                // We create a UserInfoDto object from principal
                userInfo = new UserInfoDto
                {
                    Login = login,
                    Language = Constants.DefaultValues.Language,
                };
            }

            this.userAppService.SelectDefaultLanguage(userInfo);

            var userMainRights = this.userPermissionDomainService.TranslateRolesInPermissions(userRoles, loginParam.LightToken);

            IEnumerable<TeamDto> allTeams = new List<TeamDto>();
            if (!loginParam.LightToken)
            {
                allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userMainRights);
            }

            List<string> allRoles = await this.GetRoles(loginParam, userData, userRoles, userInfo, allTeams);

            if (allRoles == null || !allRoles.Any())
            {
                this.logger.LogInformation("Unauthorized because no role found");
                return this.Unauthorized("No role found");
            }

            // translate roles in permission
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(allRoles, loginParam.LightToken);

            if (!userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                return this.Unauthorized("No permission found");
            }

            var tokenDto = new TokenDto<UserDataDto> { Login = login, Id = userInfo.Id, Permissions = userPermissions, UserData = userData };

            UserProfileDto userProfile = null;
            if (userProfileTask != null)
            {
                userProfile = userProfileTask.Result ?? new UserProfileDto { Theme = Constants.DefaultValues.Theme };
            }

            AdditionalInfoDto additionnalInfo = null;
            if (!loginParam.LightToken)
            {
                additionnalInfo = new AdditionalInfoDto { UserInfo = userInfo, UserProfile = userProfile, Teams = allTeams.ToList() };
            }

            var authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionnalInfo, loginParam.LightToken);

            return this.Ok(authInfo);
        }

        /// <summary>
        /// Gets the front end version.
        /// </summary>
        /// <returns>The front end version.</returns>
        [HttpGet("frontEndVersion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetFrontEndVersion()
        {
            return this.Ok(Constants.Application.FrontEndVersion);
        }

        private async Task<List<string>> GetRoles(LoginParamDto loginParam, UserDataDto userData, List<string> userRolesFromUserDirectory, UserInfoDto userInfo, IEnumerable<TeamDto> allTeams)
        {
            // the main roles
            var allRoles = userRolesFromUserDirectory;

            // get user rights
            if (userRolesFromUserDirectory.Contains(Constants.Role.User) || userRolesFromUserDirectory.Contains(Constants.Role.Admin))
            {
                var userRoles = await this.roleAppService.GetUserRolesAsync(userInfo.Id);
                allRoles.AddRange(userRoles);

                if (loginParam.TeamsConfig != null)
                {
                    foreach (var teamConfig in loginParam.TeamsConfig)
                    {
                        CurrentTeamDto teamLogin = loginParam.CurrentTeamLogins?.FirstOrDefault(ct => ct.TeamTypeId == teamConfig.TeamTypeId);
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
                            var teams = allTeams.Where(t => t.TeamTypeId == teamLogin.TeamTypeId);
                            var team = teams?.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                            CurrentTeamDto currentTeam = new ();
                            currentTeam.TeamTypeId = teamLogin.TeamTypeId;

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
                                var roles = await this.roleAppService.GetMemberRolesAsync(currentTeam.TeamId, userInfo.Id);
                                var roleMode = loginParam.TeamsConfig?.FirstOrDefault(r => r.TeamTypeId == currentTeam.TeamTypeId)?.RoleMode ?? RoleMode.AllRoles;

                                if (roleMode == RoleMode.AllRoles)
                                {
                                    currentTeam.CurrentRoleIds = roles.Select(r => r.Id).ToList();
                                }
                                else if (roleMode == RoleMode.SingleRole)
                                {
                                    RoleDto role = roles?.OrderByDescending(x => x.IsDefault).FirstOrDefault();
                                    if (role != null)
                                    {
                                        if (teamLogin.CurrentRoleIds != null && teamLogin.CurrentRoleIds.Count == 1 && roles.Any(s => s.Id == teamLogin.CurrentRoleIds.First()))
                                        {
                                            currentTeam.CurrentRoleIds = new List<int> { teamLogin.CurrentRoleIds.First() };
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
                                            List<int> roleIdsToSet = roles.Where(r => teamLogin.CurrentRoleIds != null && teamLogin.CurrentRoleIds.Any(tr => tr == r.Id)).Select(r => r.Id).ToList();
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
                                allRoles.AddRange(roles.Where(r => currentTeam.CurrentRoleIds.Any(id => id == r.Id)).Select(r => r.Code).ToList());

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
            }

            return allRoles;
        }
    }
}
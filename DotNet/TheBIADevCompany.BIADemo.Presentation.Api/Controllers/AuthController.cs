// <copyright file="AuthController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Api.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
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
        /// The Permission service.
        /// </summary>
        private readonly IPermissionAppService permissionAppService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="userPermissionDomainService">The User Right domain service.</param>
        /// <param name="permissionAppService">The Permission service.</param>
        /// <param name="logger">The logger.</param>
        public AuthController(
            IJwtFactory jwtFactory,
            IUserAppService userAppService,
            ITeamAppService teamAppService,
            IRoleAppService roleAppService,
            IUserPermissionDomainService userPermissionDomainService,
            IPermissionAppService permissionAppService,
            ILogger<AuthController> logger)
        {
            this.jwtFactory = jwtFactory;
            this.userAppService = userAppService;
            this.teamAppService = teamAppService;
            this.roleAppService = roleAppService;
            this.logger = logger;
            this.userPermissionDomainService = userPermissionDomainService;
            this.permissionAppService = permissionAppService;
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
            return await this.LoginDefault();
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="roleModes">role mode for the different teams.</param>
        /// <returns>The JWT if authenticated.</returns>
        [HttpPost("loginAndTeamsDefault")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginDefault(RoleModeDto[] roleModes = null)
        {
            TeamLoginDto[] teamsLogin =
            {
                new TeamLoginDto
                {
                    TeamTypeId = (int)TeamTypeId.Site,
                    TeamId = 0,
                    RoleMode = roleModes?.FirstOrDefault(r => r.TeamTypeId == (int)TeamTypeId.Site)?.roleMode ?? RoleMode.AllRoles,
                    UseDefaultRoles = true,
                    RoleIds = { },
                },

                // Begin BIADemo
                new TeamLoginDto
                {
                    TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                    TeamId = 0,
                    RoleMode = roleModes?.FirstOrDefault(r => r.TeamTypeId == (int)TeamTypeId.AircraftMaintenanceCompany)?.roleMode ?? RoleMode.AllRoles,
                    UseDefaultRoles = true,
                    RoleIds = { },
                },

                // End BIADemo
            };

            return await this.LoginOnTeams(teamsLogin);
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="teamLogins">The teams info for login.</param>
        /// <returns>
        /// The JWT if authenticated.
        /// </returns>
        [HttpPost("loginAndTeams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginOnTeams(TeamLoginDto[] teamLogins)
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
                return this.BadRequest("Incorrect login");
            }

            if (string.IsNullOrEmpty(sid))
            {
                this.logger.LogWarning("Unauthorized because bad sid");
                return this.BadRequest("Incorrect sid");
            }

            // parallel launch the get user profile
            var userProfileTask = this.userAppService.GetUserProfileAsync(login);

            // get roles
            var userRolesFromUserDirectory = await this.userAppService.GetUserDirectoryRolesAsync(sid);

            if (userRolesFromUserDirectory == null || !userRolesFromUserDirectory.Any())
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                return this.Forbid("No roles found");
            }

            // get user info
            UserInfoDto userInfo = null;
            if (userRolesFromUserDirectory.Contains(Constants.Role.User))
            {
                try
                {
                    userInfo = await this.userAppService.GetCreateUserInfoAsync(sid);
                }
                catch (Exception)
                {
                    this.logger.LogWarning("Cannot create user... Probably database is read only...");
                }

                if (userInfo != null)
                {
                    try
                    {
                        await this.userAppService.UpdateLastLoginDateAndActivate(userInfo.Id);
                    }
                    catch (Exception)
                    {
                        this.logger.LogWarning("Cannot update last login date... Probably database is read only...");
                    }
                }
            }

            if (userInfo == null)
            {
                userInfo = new UserInfoDto { Login = login, Language = Constants.DefaultValues.Language };
            }

            var userMainRights = this.userAppService.TranslateRolesInPermissions(userRolesFromUserDirectory);
            var allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userMainRights);

            List<string> allRoles = await this.GetRoles(teamLogins, userData, userRolesFromUserDirectory, userInfo, allTeams);

            if (allRoles == null || !allRoles.Any())
            {
                this.logger.LogInformation("Unauthorized because no role found");
                return this.Unauthorized("No role found");
            }

            List<string> userPermissions = null;

            // translate roles in permission
            userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(allRoles);

            // add the same permission in the id form.
            userPermissions.AddRange(this.permissionAppService.GetPermissionsIds(userPermissions).Select(id => id.ToString()));

            if (!userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                return this.Unauthorized("No permission found");
            }

            var tokenDto = new TokenDto<UserDataDto> { Login = login, Id = userInfo.Id, Permissions = userPermissions, UserData = userData };
            var userProfile = userProfileTask.Result ?? new UserProfileDto { Theme = Constants.DefaultValues.Theme };
            var additionnalInfo = new AdditionalInfoDto { UserInfo = userInfo, UserProfile = userProfile, Teams = allTeams.ToList() };

            var authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionnalInfo);

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

        private async Task<List<string>> GetRoles(TeamLoginDto[] teamLogins, UserDataDto userData, List<string> userRolesFromUserDirectory, UserInfoDto userInfo, IEnumerable<TeamDto> allTeams)
        {
            // the main roles
            var allRoles = userRolesFromUserDirectory;

            // get user rights
            if (userRolesFromUserDirectory.Contains(Constants.Role.User))
            {
                foreach (var teamLogin in teamLogins)
                {
                    var teams = allTeams.Where(t => t.TeamTypeId == teamLogin.TeamTypeId);
                    var team = teams?.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                    CurrentTeamDto currentTeam = new ();
                    currentTeam.TeamTypeId = teamLogin.TeamTypeId;

                    if (team != null)
                    {
                        if (teamLogin.TeamId > 0 && teams.Any(s => s.Id == teamLogin.TeamId))
                        {
                            currentTeam.CurrentTeamId = teamLogin.TeamId;
                            currentTeam.CurrentTeamTitle = teams.First(s => s.Id == teamLogin.TeamId).Title;
                        }
                        else
                        {
                            currentTeam.CurrentTeamId = team.Id;
                            currentTeam.CurrentTeamTitle = team.Title;
                        }
                    }

                    if (currentTeam.CurrentTeamId > 0)
                    {
                        var roles = await this.roleAppService.GetMemberRolesAsync(currentTeam.CurrentTeamId, userInfo.Id);

                        if (teamLogin.RoleMode == RoleMode.AllRoles)
                        {
                            currentTeam.CurrentRoleIds = roles.Select(r => r.Id).ToList();
                        }
                        else if (teamLogin.RoleMode == RoleMode.SingleRole)
                        {
                            RoleDto role = roles?.OrderByDescending(x => x.IsDefault).FirstOrDefault();
                            if (role != null)
                            {
                                if (teamLogin.RoleIds != null && teamLogin.RoleIds.Length == 1 && roles.Any(s => s.Id == teamLogin.RoleIds.First()))
                                {
                                    currentTeam.CurrentRoleIds = new List<int> { teamLogin.RoleIds.First() };
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
                                    List<int> roleIdsToSet = roles.Where(r => teamLogin.RoleIds != null && teamLogin.RoleIds.Any(tr => tr == r.Id)).Select(r => r.Id).ToList();
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

                        // End BIADemo
                    }
                }
            }

            return allRoles;
        }
    }
}
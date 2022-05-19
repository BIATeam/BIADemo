// <copyright file="AuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
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
        private readonly BIAClaimsPrincipal principal;

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
        /// The configuration.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAppService"/> class.
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
        public AuthAppService(
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            IUserAppService userAppService,
            ITeamAppService teamAppService,
            ILogger<AuthAppService> logger,
            IRoleAppService roleAppService,
            IUserProfileRepository userProfileRepository,
            IOptions<BiaNetSection> configuration)
        {
            this.jwtFactory = jwtFactory;
            this.principal = principal as BIAClaimsPrincipal;
            this.userPermissionDomainService = userPermissionDomainService;
            this.userAppService = userAppService;
            this.teamAppService = teamAppService;
            this.logger = logger;
            this.roleAppService = roleAppService;
            this.userProfileRepository = userProfileRepository;
            this.configuration = configuration.Value;
        }

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>
        /// AuthInfo.
        /// </returns>
        public async Task<AuthInfoDTO<UserDataDto, AdditionalInfoDto>> LoginOnTeamsAsync(IIdentity identity, LoginParamDto loginParam)
        {
            if (identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(this.principal?.GetUserLogin()))
            {
                if (identity == null)
                {
                    this.logger.LogInformation("Unauthorized because identity is null");
                }
                else if (!identity.IsAuthenticated)
                {
                    this.logger.LogInformation("Unauthorized because not authenticated");
                }
                else if (string.IsNullOrWhiteSpace(this.principal?.GetUserLogin()))
                {
                    this.logger.LogInformation("The JWT is empty");
                }

                throw new UnauthorizedException();
            }

            string login = identity.Name?.Split('\\').LastOrDefault();
            string sid = this.principal.GetSid();

            // parallel launch the get user profile
            Task<UserProfileDto> userProfileTask = null;
            if (!loginParam.LightToken)
            {
                userProfileTask = this.userProfileRepository.GetAsync(login);
            }

            UserInfoDto userInfo = await this.userAppService.GetUserInfoAsync(sid);
            List<string> userRoles = this.principal.GetUserPermissions()?.Intersect(this.configuration?.Roles?.Select(x => x.Label))?.Distinct()?.ToList();

            // If the user does not exist in the database and has no role
            if (userInfo == null && userRoles?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                throw new ForbiddenException("No roles found");
            }

            userRoles = userRoles ?? new List<string>();

            // If the user exists in the database but does not have the User role
            if (this.configuration?.Roles?.Any(x => x.Label == Constants.Role.User) == true && !userRoles.Contains(Constants.Role.User))
            {
                // we add it to him.
                userRoles.Add(Constants.Role.User);
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

            // If the user does not exist in the database
            if (userInfo == null)
            {
                // We create a UserInfoDto object from the token data
                userInfo = new UserInfoDto
                {
                    Login = login,
                    FirstName = this.principal.GetUserFirstName(),
                    LastName = this.principal.GetUserLastName(),
                    Country = this.principal.GetUserCountry(),
                };

                this.userAppService.SelectDefaultLanguage(userInfo);
            }

            List<string> userMainRights = this.userPermissionDomainService.TranslateRolesInPermissions(userRoles, loginParam.LightToken);

            IEnumerable<TeamDto> allTeams = new List<TeamDto>();
            if (!loginParam.LightToken)
            {
                allTeams = await this.teamAppService.GetAllAsync(userInfo.Id, userMainRights);
            }

            UserDataDto userData = new UserDataDto();
            List<string> allRoles = await this.GetRolesAsync(loginParam, userData, userRoles, userInfo, allTeams);

            if (allRoles == null || !allRoles.Any())
            {
                this.logger.LogInformation("Unauthorized because no role found");
                throw new UnauthorizedException("No role found");
            }

            List<string> userPermissions = null;

            // translate roles in permission
            userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(allRoles, loginParam.LightToken);

            if (!userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
            }

            TokenDto<UserDataDto> tokenDto = new TokenDto<UserDataDto> { Login = login, Id = userInfo.Id, Permissions = userPermissions, UserData = userData };

            UserProfileDto userProfile = null;
            if (userProfileTask != null)
            {
                userProfile = await userProfileTask;
                userProfile = userProfile ?? new UserProfileDto { Theme = Constants.DefaultValues.Theme };
            }

            AdditionalInfoDto additionnalInfo = null;
            if (!loginParam.LightToken)
            {
                additionnalInfo = new AdditionalInfoDto { UserInfo = userInfo, UserProfile = userProfile, Teams = allTeams.ToList() };
            }

            AuthInfoDTO<UserDataDto, AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, additionnalInfo, loginParam.LightToken);

            return authInfo;
        }

        /// <summary>
        /// Gets the roles asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="userRolesFromUserDirectory">The user roles from user directory.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="allTeams">All teams.</param>
        /// <returns>List of role.</returns>
        private async Task<List<string>> GetRolesAsync(LoginParamDto loginParam, UserDataDto userData, List<string> userRolesFromUserDirectory, UserInfoDto userInfo, IEnumerable<TeamDto> allTeams)
        {
            // the main roles
            var allRoles = userRolesFromUserDirectory;

            // get user rights
            if (userRolesFromUserDirectory.Contains(Constants.Role.User))
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

                            CurrentTeamDto currentTeam = new CurrentTeamDto();
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

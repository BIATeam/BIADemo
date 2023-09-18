// <copyright file="AuthController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Api.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
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
        private readonly ISiteAppService siteAppService;

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
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="siteAppService">The site application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="userPermissionDomainService">The User Right domain service.</param>
        /// <param name="permissionAppService">The Permission service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public AuthController(
            IJwtFactory jwtFactory,
            IUserAppService userAppService,
            ISiteAppService siteAppService,
            IRoleAppService roleAppService,
            IUserPermissionDomainService userPermissionDomainService,
            IPermissionAppService permissionAppService,
            ILogger<AuthController> logger,
            IOptions<BiaNetSection> configuration)
        {
            this.jwtFactory = jwtFactory;
            this.userAppService = userAppService;
            this.siteAppService = siteAppService;
            this.roleAppService = roleAppService;
            this.logger = logger;
            this.userPermissionDomainService = userPermissionDomainService;
            this.permissionAppService = permissionAppService;
            this.ldapDomains = configuration.Value.Authentication.LdapDomains;
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="singleRoleMode">Whether the front is configured to use a single role at a time.</param>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet("login/{singleRoleMode?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(bool singleRoleMode = false)
        {
            return await this.LoginOnSite(0, singleRoleMode);
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="singleRoleMode">Whether the front is configured to use a single role at a time.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns>
        /// The JWT if authenticated.
        /// </returns>
        [HttpGet("login/site/{siteId}/{singleRoleMode?}/{roleId?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginOnSite(int siteId, bool singleRoleMode = false, int roleId = 0)
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

            var sid = ((System.Security.Principal.WindowsIdentity)identity).User.Value;

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
            var userProfileTask = this.userAppService.GetUserProfileAsync(login);

            // Get userInfo
            UserInfoDto userInfo = await this.userAppService.GetUserInfoAsync(login);

            // get roles
            var userRoles = await this.userAppService.GetUserDirectoryRolesAsync(userInfo?.IsActive == true, sid, domain);

            // the main roles
            var allRoles = userRoles;

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
                    await this.userAppService.UpdateLastLoginDateAndActivate(userInfo.Id, userRoles?.Contains(Constants.Role.User) == true);
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

            // get user rights
            if (userRoles.Contains(Constants.Role.User))
            {
                var userMainRights = this.userAppService.TranslateRolesInPermissions(userRoles);
                var sites = await this.siteAppService.GetAllAsync(userInfo.Id, userMainRights);
                var site = sites?.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                userData.Sites = sites.Select(s => new BIA.Net.Core.Domain.Dto.Option.OptionDto { Id = s.Id, Display = s.Title }).ToList();

                if (site != null)
                {
                    if (site.IsDefault)
                    {
                        userData.DefaultSiteId = site.Id;
                    }

                    if (siteId > 0 && sites.Any(s => s.Id == siteId))
                    {
                        userData.CurrentSiteId = siteId;
                        userData.CurrentSiteTitle = sites.First(s => s.Id == siteId).Title;
                    }
                    else
                    {
                        userData.CurrentSiteId = site.Id;
                        userData.CurrentSiteTitle = site.Title;
                    }
                }

                if (userData.CurrentSiteId > 0)
                {
                    var roles = await this.roleAppService.GetMemberRolesAsync(userData.CurrentSiteId, userInfo.Id);
                    userData.Roles = roles.ToList();

                    if (singleRoleMode)
                    {
                        var role = roles?.OrderByDescending(x => x.IsDefault).FirstOrDefault();
                        if (role != null)
                        {
                            if (role.IsDefault)
                            {
                                userData.DefaultRoleId = role.Id;
                            }

                            if (roleId > 0 && roles.Any(s => s.Id == roleId))
                            {
                                userData.CurrentRoleIds = new List<int> { roleId };
                            }
                            else
                            {
                                userData.CurrentRoleIds = new List<int> { role.Id };
                            }
                        }
                        else
                        {
                            userData.CurrentRoleIds = new List<int>();
                        }
                    }
                    else
                    {
                        userData.CurrentRoleIds = roles.Select(r => r.Id).ToList();
                    }

                    // add the sites roles (filter if singleRole mode is used)
                    allRoles.AddRange(userData.Roles.Where(r => userData.CurrentRoleIds.Any(id => id == r.Id)).Select(r => r.Code).ToList());

                    // add computed roles (can be customized)
                    allRoles.Add(Constants.Role.SiteMember);
                }
            }

            if (allRoles == null || !allRoles.Any())
            {
                this.logger.LogInformation("Unauthorized because no role found");
                return this.Unauthorized("No role found");
            }

            // translate roles in permission
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(allRoles);

            // add the same permission in the id form.
            userPermissions.AddRange(this.permissionAppService.GetPermissionsIds(userPermissions).Select(id => id.ToString()));

            if (userPermissions == null || !userPermissions.Any())
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                return this.Unauthorized("No permission found");
            }

            var claimsIdentity = await Task.FromResult(this.jwtFactory.GenerateClaimsIdentity(login, userInfo.Id, userPermissions, userData));
            if (claimsIdentity == null)
            {
                this.logger.LogInformation("Unauthorized because claimsIdentity is null");
                return this.Unauthorized("No rights found");
            }

            var userProfile = userProfileTask.Result ?? new UserProfileDto { Theme = Constants.DefaultValues.Theme };

            var token = await this.jwtFactory.GenerateJwtAsync(claimsIdentity, new { UserInfo = userInfo, UserProfile = userProfile, UserData = userData });

            return this.Ok(token);
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
    }
}
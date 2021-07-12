// <copyright file="AuthController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Api.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

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
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="siteAppService">The site application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="logger">The logger.</param>
        public AuthController(IJwtFactory jwtFactory, IUserAppService userAppService, ISiteAppService siteAppService, IRoleAppService roleAppService, ILogger<AuthController> logger)
        {
            this.jwtFactory = jwtFactory;
            this.userAppService = userAppService;
            this.siteAppService = siteAppService;
            this.roleAppService = roleAppService;
            this.logger = logger;
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
                userInfo = await this.userAppService.GetCreateUserInfoAsync(sid);
                try
                {
                    await this.userAppService.UpdateLastLoginDateAndActivate(userInfo.Id);
                }
                catch (Exception)
                {
                    this.logger.LogWarning("Cannot update last login date... Probably database is read only...");
                }
            }

            if (userInfo == null)
            {
                userInfo = new UserInfoDto { Login = login, Language = Constants.DefaultValues.Language };
            }

            // get user rights
            List<string> userRights = null;
            if (userRolesFromUserDirectory.Contains(Constants.Role.User))
            {
                if (siteId < 1)
                {
                    var userMainRights = this.userAppService.TranslateRolesInRights(userRolesFromUserDirectory);
                    var sites = await this.siteAppService.GetAllAsync(userInfo.Id, userMainRights);
                    var site = sites?.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                    if (site != null)
                    {
                        siteId = site.Id;
                    }
                }

                if (singleRoleMode && roleId < 1)
                {
                    var roles = await this.roleAppService.GetMemberRolesAsync(siteId, userInfo.Id);

                    var role = roles?.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                    if (role != null)
                    {
                        roleId = role.Id;
                    }
                }

                if (siteId > 0)
                {
                    userRights = await this.userAppService.GetRightsForUserAsync(userRolesFromUserDirectory, sid, siteId, roleId);
                    if (userRights == null || !userRights.Any())
                    {
                        this.logger.LogInformation("Unauthorized because no user rights for site : " + siteId);
                        return this.Unauthorized("You don't have access to this site");
                    }
                }
            }

            // For admin and non user
            if (userRights == null)
            {
                userRights = await this.userAppService.GetRightsForUserAsync(userRolesFromUserDirectory, sid, 0, 0);
            }

            if (userRights == null || !userRights.Any())
            {
                this.logger.LogInformation("Unauthorized because No rights found");
                return this.Unauthorized("No rights found");
            }

            // user data
            var userData = new { CurrentSiteId = 0, CurrentRoleId = 0 };

            if (siteId > 0)
            {
                userData = new
                {
                    CurrentSiteId = siteId,
                    CurrentRoleId = roleId,
                };
            }

            var claimsIdentity = await Task.FromResult(this.jwtFactory.GenerateClaimsIdentity(login, userInfo.Id, userRights, userData));
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
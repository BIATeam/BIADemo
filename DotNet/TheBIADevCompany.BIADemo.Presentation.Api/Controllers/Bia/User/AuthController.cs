// <copyright file="AuthController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.User
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NLog;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Base;

    /// <summary>
    /// AAA The API controller used to authenticate users.
    /// </summary>
    [SwaggerControllerOrder(1)]
    public class AuthController : AuthControllerBase
    {
#if BIA_FRONT_FEATURE || BIA_BACK_TO_BACK_AUTH
        /// <summary>
        /// The authentication service.
        /// </summary>
        private readonly IAuthAppService authService;

        /// <summary>
        /// The team service.
        /// </summary>
        private readonly ITeamAppService teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthController(IAuthAppService authService, ITeamAppService teamService)
        {
            this.authService = authService;
            this.teamService = teamService;
        }
#endif

#if BIA_FRONT_FEATURE

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="lightToken">If true return a token without team permission.</param>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(bool lightToken = true)
        {
            // used only by swagger.
            var loginParam = new LoginParamDto
            {
                TeamsConfig = this.teamService.GetTeamsConfig(),
                CurrentTeamLogins = null,
                LightToken = lightToken,
                FineGrainedPermission = !lightToken,
                AdditionalInfos = !lightToken,
                IsFirstLogin = true,
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginOnTeams(LoginParamDto loginParam)
        {
            try
            {
                AuthInfoDto<AdditionalInfoDto> authInfo = await this.authService.LoginOnTeamsAsync(loginParam);

                return this.Ok(authInfo);
            }
            catch (UnauthorizedException ex)
            {
                return this.Unauthorized(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return this.Forbid(ex.Message);
            }
        }

        /// <summary>
        /// Gets the front end version.
        /// </summary>
        /// <returns>The front end version.</returns>
        [HttpGet("frontEndVersion")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public IActionResult GetFrontEndVersion()
        {
            return this.Ok(Constants.Application.FrontEndVersion);
        }
#endif
#if BIA_BACK_TO_BACK_AUTH

        /// <summary>
        /// The login action.
        /// </summary>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetToken()
        {
            try
            {
                string token = await this.authService.LoginAsync();
                return this.Ok(token);
            }
            catch (UnauthorizedException ex)
            {
                return this.Unauthorized(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return this.Forbid(ex.Message);
            }
        }
#endif
    }
}
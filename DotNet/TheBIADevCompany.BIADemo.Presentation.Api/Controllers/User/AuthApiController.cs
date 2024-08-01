// <copyright file="AuthApiController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Base;

    /// <summary>
    /// The API controller used to authenticate users.
    /// </summary>
    public class AuthApiController : AuthControllerBase
    {
        /// <summary>
        /// The authentication service.
        /// </summary>
        private readonly IAuthApiAppService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthApiController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthApiController(IAuthApiAppService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="loginParam">The parameters for login.</param>
        /// <returns>
        /// The JWT if authenticated.
        /// </returns>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login()
        {
            try
            {
                string token = await this.authService.LoginAsync();

                if (string.IsNullOrWhiteSpace(token))
                {
                    return this.Unauthorized();
                }

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
    }
}
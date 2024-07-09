// <copyright file="BiaControllerBase.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Presentation.Api.Controllers.Base
{
    using System.Linq;
    using System.Security.Claims;
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The base class for BIA controllers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = AuthenticationConfiguration.JwtBearerDefault)]
    public abstract class BiaControllerBase : ControllerBase
    {
        /// <summary>
        /// Check autorize based on teamTypeId.
        /// </summary>
        /// <param name="role">the role.</param>
        /// <returns>true if authorized.</returns>
        protected bool IsAuthorize(string role)
        {
            if (!this.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role))
            {
                return false;
            }

            return true;
        }
    }
}
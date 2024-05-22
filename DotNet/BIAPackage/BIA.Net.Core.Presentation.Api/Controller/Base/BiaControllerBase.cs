// <copyright file="BiaControllerBase.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Presentation.Api.Controllers.Base
{
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// The base class for BIA controllers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = StartupConfiguration.JwtBearerDefault)]
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
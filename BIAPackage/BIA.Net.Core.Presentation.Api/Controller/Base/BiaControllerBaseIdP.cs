// <copyright file="BiaControllerBaseIdP.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Presentation.Api.Controllers.Base
{
    using BIA.Net.Core.Presentation.Common.Authentication;
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
    [Authorize(AuthenticationSchemes = StartupConfiguration.JwtBearerIdentityProvider)]
    public abstract class BiaControllerBaseIdP : ControllerBase
    {
    }
}
// <copyright file="AuthControllerBase.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Base
{
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Auth Controller Base.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Policy = BiaAuthorizationPolicyProvider.DefaultBiaAuthorizationPolicyName)]
    public abstract class AuthControllerBase : ControllerBase
    {
    }
}

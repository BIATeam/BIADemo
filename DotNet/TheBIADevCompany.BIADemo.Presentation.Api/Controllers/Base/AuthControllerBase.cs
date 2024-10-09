// <copyright file="AuthControllerBase.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Base
{
    using BIA.Net.Core.Presentation.Api.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Auth Controller Base.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ServiceFilter(typeof(BiaAuthorizeFilter))]
    public abstract class AuthControllerBase : ControllerBase
    {
    }
}

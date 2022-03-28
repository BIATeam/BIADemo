// <copyright file="PermissionsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage permissions.
    /// </summary>
    public class PermissionsController : BiaControllerBase
    {
        /// <summary>
        /// The service permission.
        /// </summary>
        private readonly IPermissionAppService permissionService;

        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsController"/> class.
        /// </summary>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="principal">The claims principal.</param>
        public PermissionsController(IPermissionAppService permissionService, IPrincipal principal)
        {
            this.permissionService = permissionService;
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of production sites.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Permissions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.permissionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
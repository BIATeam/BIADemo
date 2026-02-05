// <copyright file="RolesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.User
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage roles.
    /// </summary>
    public class RolesController : BiaControllerBase
    {
        /// <summary>
        /// The service role option.
        /// </summary>
        private readonly IRoleOptionAppService roleOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleOptionService">The role option service.</param>
        /// <param name="principal">The claims principal.</param>
        public RolesController(IRoleOptionAppService roleOptionService, IPrincipal principal)
        {
            this.roleOptionService = roleOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <param name="teamTypeId">The team type id.</param>
        /// <returns>The list of production sites.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Roles_Options))]
        public async Task<IActionResult> GetAllOptions(int teamTypeId)
        {
            var results = await this.roleOptionService.GetAllOptionsAsync(teamTypeId);
            return this.Ok(results);
        }
    }
}
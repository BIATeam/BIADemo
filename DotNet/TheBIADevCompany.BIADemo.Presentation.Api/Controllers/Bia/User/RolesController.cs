// <copyright file="RolesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.User
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Bia.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage roles.
    /// </summary>
    public class RolesController : BiaControllerBase
    {
        /// <summary>
        /// The service role.
        /// </summary>
        private readonly IRoleAppService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        /// <param name="principal">The claims principal.</param>
        public RolesController(IRoleAppService roleService, IPrincipal principal)
        {
            this.roleService = roleService;
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
        [Authorize(Roles = Rights.Roles.Options)]
        public async Task<IActionResult> GetAllOptions(int teamTypeId)
        {
            var results = await this.roleService.GetAllOptionsAsync(teamTypeId);
            return this.Ok(results);
        }
    }
}
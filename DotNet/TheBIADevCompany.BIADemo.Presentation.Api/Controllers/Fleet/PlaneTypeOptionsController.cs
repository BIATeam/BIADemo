// BIADemo only
// <copyright file="PlaneTypeOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// The API controller used to manage plane type options.
    /// </summary>
    public class PlaneTypeOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The plane type application service.
        /// </summary>
        private readonly IPlaneTypeOptionAppService planeTypeOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTypeOptionsController"/> class.
        /// </summary>
        /// <param name="planeTypeOptionService">The plane type application service.</param>
        public PlaneTypeOptionsController(IPlaneTypeOptionAppService planeTypeOptionService)
        {
            this.planeTypeOptionService = planeTypeOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of plane types.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.PlaneType_Options))]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.planeTypeOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
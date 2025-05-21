// BIADemo only
// <copyright file="PlaneOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System.Threading.Tasks;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage plane options.
    /// </summary>
    public class PlaneOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly IPlaneOptionAppService planeOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneOptionsController"/> class.
        /// </summary>
        /// <param name="planeOptionService">The plane application service.</param>
        public PlaneOptionsController(IPlaneOptionAppService planeOptionService)
        {
            this.planeOptionService = planeOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of planes.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.PlaneOptions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.planeOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}

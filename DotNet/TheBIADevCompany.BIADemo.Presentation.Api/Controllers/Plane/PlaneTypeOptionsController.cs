// BIADemo only
// <copyright file="PlaneTypeOptionsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Plane
{
    using System.Threading.Tasks;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage PlaneType options.
    /// </summary>
    public class PlaneTypeOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The PlaneType application service.
        /// </summary>
        private readonly IPlaneTypeOptionAppService airportOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTypeOptionsController"/> class.
        /// </summary>
        /// <param name="airportOptionService">The PlaneType application service.</param>
        public PlaneTypeOptionsController(IPlaneTypeOptionAppService airportOptionService)
        {
            this.airportOptionService = airportOptionService;
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
        [Authorize(Roles = Rights.PlanesTypes.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.airportOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
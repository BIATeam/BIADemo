// BIADemo only
// <copyright file="AirportOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage Airport options.
    /// </summary>
    public class AirportOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The Airport application service.
        /// </summary>
        private readonly IAirportOptionAppService airportOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AirportOptionsController"/> class.
        /// </summary>
        /// <param name="airportOptionService">The Airport application service.</param>
        public AirportOptionsController(IAirportOptionAppService airportOptionService)
        {
            this.airportOptionService = airportOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of airports.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Airports.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.airportOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
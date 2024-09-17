// BIADemo only
// <copyright file="CountriesController.cs" company="TheBIADevCompany">
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
    /// The API controller used to manage Countries.
    /// </summary>
    public class CountriesController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly ICountryAppService countryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        /// <param name="countryService">The country application service.</param>
        public CountriesController(ICountryAppService countryService)
        {
            this.countryService = countryService;
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
        [Authorize(Roles = Rights.Countries.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.countryService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
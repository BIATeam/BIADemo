// BIADemo only
// <copyright file="CountryOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Maintenance
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Maintenance;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage country options.
    /// </summary>
    public class CountryOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The country application service.
        /// </summary>
        private readonly ICountryOptionAppService countryOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryOptionsController"/> class.
        /// </summary>
        /// <param name="countryOptionService">The country application service.</param>
        public CountryOptionsController(ICountryOptionAppService countryOptionService)
        {
            this.countryOptionService = countryOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of countries.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.CountryOptions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.countryOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
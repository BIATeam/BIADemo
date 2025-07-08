// <copyright file="LanguagesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Translation;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller used to manage languages.
    /// </summary>
    public class LanguagesController : BiaControllerBase
    {
        /// <summary>
        /// The service language.
        /// </summary>
        private readonly ILanguageAppService languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguagesController"/> class.
        /// </summary>
        /// <param name="languageService">The language service.</param>
        public LanguagesController(ILanguageAppService languageService)
        {
            this.languageService = languageService;
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
        [Authorize(Roles = BiaRights.Languages.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.languageService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
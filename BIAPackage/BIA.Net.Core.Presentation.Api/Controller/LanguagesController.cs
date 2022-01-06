// <copyright file="LanguagesController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Controller
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Translation;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
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
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguagesController"/> class.
        /// </summary>
        /// <param name="languageService">The language service.</param>
        /// <param name="principal">The claims principal.</param>
        public LanguagesController(ILanguageAppService languageService, IPrincipal principal)
        {
            this.languageService = languageService;
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
        [Authorize(Roles = BIARights.Languages.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.languageService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
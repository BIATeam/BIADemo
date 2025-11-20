// <copyright file="AnnoucementTypeOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Annoucement
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Annoucement;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage annoucement type options.
    /// </summary>
    public class AnnoucementOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The annoucement type application service.
        /// </summary>
        private readonly IAnnoucementOptionAppService annoucementTypeOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnoucementOptionsController"/> class.
        /// </summary>
        /// <param name="annoucementTypeOptionService">The annoucement type application service.</param>
        public AnnoucementOptionsController(IAnnoucementOptionAppService annoucementTypeOptionService)
        {
            this.annoucementTypeOptionService = annoucementTypeOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of annoucement types.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AnnoucementTypeOptions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.annoucementTypeOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}

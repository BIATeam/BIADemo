// BIADemo only
// <copyright file="SiteOptionsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Site
{
    using System.Threading.Tasks;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage site options.
    /// </summary>
    public class SiteOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The site application service.
        /// </summary>
        private readonly ISiteOptionAppService siteOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteOptionsController"/> class.
        /// </summary>
        /// <param name="siteOptionService">The site application service.</param>
        public SiteOptionsController(ISiteOptionAppService siteOptionService)
        {
            this.siteOptionService = siteOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of sites.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Sites.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.siteOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}

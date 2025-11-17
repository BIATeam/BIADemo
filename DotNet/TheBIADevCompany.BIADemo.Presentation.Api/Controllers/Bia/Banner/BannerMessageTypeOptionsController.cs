// <copyright file="BannerMessageTypeOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Banner
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Banner;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage banner message type options.
    /// </summary>
    public class BannerMessageTypeOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The banner message type application service.
        /// </summary>
        private readonly IBannerMessageTypeOptionAppService bannerMessageTypeOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessageTypeOptionsController"/> class.
        /// </summary>
        /// <param name="bannerMessageTypeOptionService">The banner message type application service.</param>
        public BannerMessageTypeOptionsController(IBannerMessageTypeOptionAppService bannerMessageTypeOptionService)
        {
            this.bannerMessageTypeOptionService = bannerMessageTypeOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of banner message types.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessageTypeOptions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.bannerMessageTypeOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}

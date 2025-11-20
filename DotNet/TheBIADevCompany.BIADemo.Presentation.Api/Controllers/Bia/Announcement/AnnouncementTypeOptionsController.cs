// <copyright file="AnnouncementTypeOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Announcement
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Announcement;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage announcement type options.
    /// </summary>
    public class AnnouncementTypeOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The announcement type application service.
        /// </summary>
        private readonly IAnnouncementTypeOptionAppService announcementTypeOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementTypeOptionsController"/> class.
        /// </summary>
        /// <param name="announcementTypeOptionService">The announcement type application service.</param>
        public AnnouncementTypeOptionsController(IAnnouncementTypeOptionAppService announcementTypeOptionService)
        {
            this.announcementTypeOptionService = announcementTypeOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of announcement types.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AnnouncementTypeOptions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.announcementTypeOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}

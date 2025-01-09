// <copyright file="ProfileImageController.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using System.Drawing;
    using System.IO;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The API controller used to manage profile image.
    /// </summary>
    public class ProfileImageController : BiaControllerBase
    {
        /// <summary>
        /// The profile image service.
        /// </summary>
        private readonly IProfileImageService profileImageService;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IOptions<BiaNetSection> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileImageController"/> class.
        /// </summary>
        /// <param name="profileImageService">The profile image service.</param>
        /// <param name="configuration">The configuration of the application.</param>
        public ProfileImageController(IProfileImageService profileImageService, IOptions<BiaNetSection> configuration)
        {
            this.profileImageService = profileImageService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get the profile image.
        /// </summary>
        /// <returns>Bytes of the image.</returns>
        [HttpGet("get")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = BiaRights.ProfileImage.Get)]
        public async Task<IActionResult> Get()
        {
            var image = await this.profileImageService.GetAsync(this.configuration.Value.ProfileConfiguration.ProfileImageUrlOrPath);

            if (image != null)
            {
                // Return the image as a file response
                return this.File(image, "image/jpeg");
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}

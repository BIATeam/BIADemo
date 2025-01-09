// <copyright file="ProfileImageService.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The service for profileImage.
    /// </summary>
    public class ProfileImageService : IProfileImageService
    {
        private readonly BiaClaimsPrincipal biaClaimsPrincipal;
        private readonly IImageUrlRepository imageUrlRepository;
        private readonly IFileRepository fileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileImageService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="principal">The claims of the current user.</param>
        /// <param name="imageUrlRepository">The repository to access an image from URL.</param>
        /// <param name="fileRepository">The repository to access a file in the system.</param>
        public ProfileImageService(IPrincipal principal, IImageUrlRepository imageUrlRepository, IFileRepository fileRepository)
        {
            this.biaClaimsPrincipal = principal as BiaClaimsPrincipal;
            this.imageUrlRepository = imageUrlRepository;
            this.fileRepository = fileRepository;
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetAsync(string pathOrUrl)
        {
            string updatedPathOrUrl = pathOrUrl.Replace("{login}", this.biaClaimsPrincipal.GetUserLogin());
            if (Uri.TryCreate(updatedPathOrUrl, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                // Get image from URL
                return await this.imageUrlRepository.GetImageBytesAsync(updatedPathOrUrl);
            }
            else
            {
                // Get image from file path
                return await this.fileRepository.GetImageBytesAsync(updatedPathOrUrl);
            }
        }
    }
}

// <copyright file="FilesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The files controller.
    /// </summary>
    public class FilesController : BiaControllerBase
    {
        private readonly IBiaFileDownloaderService biaFileDownloaderService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipal;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesController"/> class.
        /// </summary>
        /// <param name="biaFileDownloaderService">The file downloader service.</param>
        /// <param name="biaClaimsPrincipal">The principal claims.</param>
        public FilesController(IBiaFileDownloaderService biaFileDownloaderService, IBiaClaimsPrincipalService biaClaimsPrincipal)
        {
            this.biaFileDownloaderService = biaFileDownloaderService;
            this.biaClaimsPrincipal = biaClaimsPrincipal;
        }

        /// <summary>
        /// Download the file with the specified guid and token.
        /// </summary>
        /// <param name="guid">The file GUID to download.</param>
        /// <param name="token">The token that authorize the download.</param>
        /// <returns>The file content result.</returns>
        [HttpGet("{guid}/[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Download([FromRoute] string guid, [FromQuery] string token)
        {
            try
            {
                var fileDownloadData = await this.biaFileDownloaderService.GetFileDownloadData(Guid.Parse(guid), token);
                var fileContent = await System.IO.File.ReadAllBytesAsync(fileDownloadData.FilePath);
                return this.File(fileContent, fileDownloadData.FileContentType, fileDownloadData.FileName);
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get a download token for the file with the specified guid. The token is used to authorize the download of the file and is valid for a limited time. The user must have the necessary permissions to get the download token for the file.
        /// </summary>
        /// <param name="guid">The file GUID to download and generate the token for.</param>
        /// <returns>The generated token.</returns>
        [HttpGet("{guid}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDownloadToken([FromRoute] string guid)
        {
            try
            {
                var token = await this.biaFileDownloaderService.GenerateDownloadToken(Guid.Parse(guid), this.biaClaimsPrincipal.GetUserId());
                return this.Ok(token);
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}

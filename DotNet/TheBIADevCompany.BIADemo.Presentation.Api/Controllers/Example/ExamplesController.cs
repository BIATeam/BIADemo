// BIADemo only
// <copyright file="ExamplesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Example
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Error;

    /// <summary>
    /// The API controller used to manage examples.
    /// </summary>
    public class ExamplesController : BiaControllerBase
    {
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;
        private readonly IBiaFileDownloaderService fileDownloaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamplesController"/> class.
        /// </summary>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        /// <param name="fileDownloaderService">The BIA file downloader service.</param>
        public ExamplesController(IBiaClaimsPrincipalService biaClaimsPrincipalService, IBiaFileDownloaderService fileDownloaderService)
        {
            this.biaClaimsPrincipalService = biaClaimsPrincipalService;
            this.fileDownloaderService = fileDownloaderService;
        }

        /// <summary>
        /// Throw unhandled exception.
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        /// <exception cref="BadHttpRequestException">Thrown exception.</exception>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GenerateUnhandledError()
        {
            throw new BadHttpRequestException("Unhandled error");
        }

        /// <summary>
        /// Throw handled exception.
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        /// <exception cref="FrontUserException">Thrown exception.</exception>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult GenerateHandledError()
        {
            throw FrontUserException.Create(ErrorId.HangfireHandledError);
        }

        /// <summary>
        /// Prepare the download of example file.
        /// </summary>
        /// <returns>No content.</returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PrepareDownloadFileExample()
        {
            var currentAssemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var downloadFileExamplePath = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), "Resources", "DownloadFileExample.txt");
            var downloadHugeFileExamplePath = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), "Resources", "DownloadHugeFileExample.zip");

            this.fileDownloaderService.PrepareDownload(
                () => Task.FromResult(new FileDownloadDataDto() { FilePath = downloadFileExamplePath, FileContentType = "text/plain; charset=utf-8", FileName = "FileExample.txt" }),
                // () => Task.FromResult(new FileDownloadDataDto() { FilePath = downloadHugeFileExamplePath, FileContentType = "application/zip", FileName = "HugeFileExample.zip" }),
                this.biaClaimsPrincipalService.GetUserId());

            return this.NoContent();
        }
    }
}

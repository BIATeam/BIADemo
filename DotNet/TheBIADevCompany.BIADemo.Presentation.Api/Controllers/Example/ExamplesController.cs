// BIADemo only
// <copyright file="ExamplesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Example
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Example;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Error;

    /// <summary>
    /// The API controller used to manage examples.
    /// </summary>
    public class ExamplesController : BiaControllerBase
    {
        private readonly IExampleAppService exampleAppService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamplesController"/> class.
        /// </summary>
        /// <param name="exampleAppService">The example application service.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public ExamplesController(IExampleAppService exampleAppService, IBiaClaimsPrincipalService biaClaimsPrincipalService)
        {
            this.exampleAppService = exampleAppService;
            this.biaClaimsPrincipalService = biaClaimsPrincipalService;
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
        /// Triggers a notification indicating that a file is ready for download for the current user.
        /// </summary>
        /// <returns>A 204 No Content response.</returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GenerateFileDownloadNotification()
        {
            await this.exampleAppService.NotifyDownloadReadyFileExample(this.biaClaimsPrincipalService.GetUserId());
            return this.NoContent();
        }
    }
}

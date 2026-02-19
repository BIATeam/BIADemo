// BIADemo only
// <copyright file="ExamplesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Example
{
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Error;

    /// <summary>
    /// The API controller used to manage examples.
    /// </summary>
    public class ExamplesController : BiaControllerBase
    {
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
    }
}

// <copyright file="LogsController.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Presentation.Api.Controllers
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The API controller used to manage users.
    /// </summary>
    public class LogsController : BiaControllerBaseNoToken
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<LogsController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LogsController(ILogger<LogsController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Create a log according to the information in parameter.
        /// </summary>
        /// <param name="log">The log information (message, level, etc...)</param>
        /// <returns>A return code.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateLog(LogDto log)
        {
            if (log == null)
            {
                return this.BadRequest();
            }

            string logMessage = $"From Angular file {log.FileName} one line {log.LineNumber} : {log.Message}";

            switch (log.Level)
            {
                case NgxLogLevel.Trace:
                    this.logger.LogTrace(logMessage);
                    break;

                case NgxLogLevel.Debug:
                    this.logger.LogDebug(logMessage);
                    break;

                case NgxLogLevel.Info:
                    this.logger.LogInformation(logMessage);
                    break;

                case NgxLogLevel.Warning:
                    this.logger.LogWarning(logMessage);
                    break;

                case NgxLogLevel.Error:
                    this.logger.LogError(logMessage);
                    break;

                case NgxLogLevel.Fatal:
                    this.logger.LogCritical(logMessage);
                    break;

                default:
                    this.logger.LogTrace(logMessage);
                    break;
            }

            return this.Ok();
        }

        /// <summary>
        /// Ping to test response.
        /// </summary>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet("ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            return this.Ok(System.Net.Dns.GetHostName() + " Ping OK : " + DateTime.Now);
        }
    }
}
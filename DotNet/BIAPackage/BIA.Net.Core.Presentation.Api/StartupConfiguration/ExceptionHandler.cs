// <copyright file="ExceptionHandler.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    /// <summary>
    /// The class containing configuration for authentication.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Configures the API exception handler.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="isDevelopment">Indicates if the current host environment is development.</param>
        public static void ConfigureApiExceptionHandler(this IApplicationBuilder app, bool isDevelopment)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                var logger = app.ApplicationServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(typeof(ExceptionHandler).FullName);

                exceptionHandlerApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    if (exception != null)
                    {
                        if (exception is FrontUserException frontUserEx)
                        {
                            logger.LogWarning(frontUserEx, $"A {nameof(FrontUserException)} has been raised, see details below.");

                            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                            var errorMessage = frontUserEx.Message;
                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = isDevelopment ? exception.GetBaseException().Message : "Internal server error";
                            }

                            var formatedErrorMessage = frontUserEx.ErrorMessageParameters.Length > 0 ? string.Format(errorMessage, frontUserEx.ErrorMessageParameters) : errorMessage;
                            await context.Response.WriteAsJsonAsync(new HttpErrorReport((int)frontUserEx.ErrorMessageKey, formatedErrorMessage));
                            return;
                        }

                        logger.LogError(exception, $"An exception has been raised, see details below.");
                        if (!isDevelopment)
                        {
                            // We don't communicate sensitive error information to clients.
                            // Serving errors is a security risk. We just send a simple error message.
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("Internal server error");
                        }
                    }
                });
            });
        }
    }
}
// <copyright file="ExceptionHandler.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

#pragma warning disable BIA001 // Forbidden reference to Domain layer in Presentation layer
namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using BIA.Net.Core.Common.Error;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Service;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
                        var userContext = context.RequestServices.GetRequiredService<UserContext>();
                        var internalServerErrorMessage = BiaErrorMessage.GetMessage(BiaErrorId.InternalServerError, userContext.LanguageId);
                        internalServerErrorMessage = string.IsNullOrWhiteSpace(internalServerErrorMessage) ? "Internal server error" : internalServerErrorMessage;

                        if (exception is FrontUserException frontUserEx)
                        {
                            logger.LogWarning(frontUserEx, $"A {nameof(FrontUserException)} has been raised, see details below.");

                            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                            var errorMessage = frontUserEx.ErrorId == (int)BiaErrorId.Unknown ? frontUserEx.Message : BiaErrorMessage.GetMessage(frontUserEx.ErrorId, userContext.LanguageId);
                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = isDevelopment ? exception.GetBaseException().Message : internalServerErrorMessage;
                            }

                            var formattedErrorMessage = frontUserEx.ErrorMessageParameters.Length > 0 ? string.Format(errorMessage, frontUserEx.ErrorMessageParameters) : errorMessage;
                            await context.Response.WriteAsJsonAsync(new HttpErrorReport(frontUserEx.ErrorId, formattedErrorMessage));
                            return;
                        }

                        logger.LogError(exception, $"An exception has been raised, see details below.");
                        if (!isDevelopment)
                        {
                            // We don't communicate sensitive error information to clients.
                            // Serving errors is a security risk. We just send a simple error message.
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync(internalServerErrorMessage);
                        }
                    }
                });
            });
        }
    }
}
#pragma warning restore BIA001 // Forbidden reference to Domain layer in Presentation layer
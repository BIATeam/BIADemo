// <copyright file="ExceptionHandler.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// The class containing configuration for authentication.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Configures the API exception handler.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void ConfigureApiExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    if (exception != null)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        // We don't communicate sensitive error information to clients.
                        // Serving errors is a security risk. We just send a simple error message.
                        await context.Response.WriteAsync("Internal server error");
                    }
                });
            });
        }
    }
}
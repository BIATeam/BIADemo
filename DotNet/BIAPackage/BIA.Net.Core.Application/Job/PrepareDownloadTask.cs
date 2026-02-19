// <copyright file="PrepareDownloadTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Job
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.File;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Hangfire task to prepare a file download in background and notify the user when it's ready.
    /// </summary>
    [AutomaticRetry(Attempts = 0, LogEvents = true)]
    internal class PrepareDownloadTask : BaseJob
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IBiaFileDownloaderService fileDownloaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareDownloadTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="fileDownloaderService">The file downloader service.</param>
        public PrepareDownloadTask(IConfiguration configuration, ILogger<PrepareDownloadTask> logger, IServiceProvider serviceProvider, IBiaFileDownloaderService fileDownloaderService)
            : base(configuration, logger)
        {
            this.serviceProvider = serviceProvider;
            this.fileDownloaderService = fileDownloaderService;
        }

        /// <summary>
        /// Runs the task to prepare the file download by invoking a specific method on a DI-registered service.
        /// This overload is used when the generation logic resides in the calling application service rather
        /// than in a dedicated <see cref="IBiaBackgroundFileGeneratorService"/> implementation.
        /// </summary>
        /// <param name="serviceTypeName">Assembly-qualified name of the DI-registered service type.</param>
        /// <param name="methodName">Name of the method to invoke on the service.</param>
        /// <param name="serializedArgs">JSON-serialized array of argument values.</param>
        /// <param name="serializedArgTypes">JSON-serialized array of argument type assembly-qualified names.</param>
        /// <param name="requestedByUserId">The user who requested the download.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Run(string serviceTypeName, string methodName, string serializedArgs, string serializedArgTypes, int requestedByUserId)
        {
            try
            {
                var serviceType = Type.GetType(serviceTypeName) ?? throw new InvalidOperationException($"Could not resolve type '{serviceTypeName}'");

                var argTypes = JsonConvert.DeserializeObject<string[]>(serializedArgTypes)
                    .Select(t => Type.GetType(t) ?? throw new InvalidOperationException($"Could not resolve argument type '{t}'"))
                    .ToArray();

                var method = serviceType.GetMethod(methodName, argTypes) ?? throw new InvalidOperationException($"Method '{methodName}' not found on type '{serviceType.Name}'");

                var rawArgs = JsonConvert.DeserializeObject<object[]>(serializedArgs);
                var typedArgs = rawArgs.Zip(argTypes, (arg, argType) => arg == null ? null : JsonConvert.DeserializeObject(JsonConvert.SerializeObject(arg), argType)).ToArray();

                var service = this.serviceProvider.GetRequiredService(serviceType);
                var fileDownloadDataDto = await (Task<FileDownloadDataDto>)method.Invoke(service, typedArgs);

                await this.fileDownloaderService.NotifyDownloadReadyAsync(fileDownloadDataDto, requestedByUserId);
            }
            catch (Exception ex)
            {
                if (this.Logger.IsEnabled(LogLevel.Error))
                {
                    this.Logger.LogError(ex, "Error while preparing download for user {UserId}", requestedByUserId);
                }

                throw;
            }
        }
    }
}

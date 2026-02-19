// <copyright file="PrepareDownloadTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Job
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.User.Entities;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
        /// Runs the task to prepare the file download.
        /// </summary>
        /// <param name="generatorType">The type of the background file generator service.</param>
        /// <param name="requestedByUser">The user who requested the download.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Run(Type generatorType, BaseEntityUser requestedByUser)
        {
            try
            {
                if (!typeof(IBiaBackgroundFileGeneratorService).IsAssignableFrom(generatorType))
                {
                    throw new InvalidOperationException($"Type {generatorType.Name} does not implement {nameof(IBiaBackgroundFileGeneratorService)}");
                }

                var generator = (IBiaBackgroundFileGeneratorService)this.serviceProvider.GetRequiredService(generatorType);
                var fileDownloadDataDto = await generator.GenerateAsync();
                fileDownloadDataDto.RequestByUser = new OptionDto { Id = requestedByUser.Id, Display = requestedByUser.Login };
                fileDownloadDataDto.RequestDateTime = DateTime.UtcNow;

                await this.fileDownloaderService.NotifyDownloadReadyAsync(fileDownloadDataDto, requestedByUser);
            }
            catch (Exception ex)
            {
                if (this.Logger.IsEnabled(LogLevel.Error))
                {
                    this.Logger.LogError(ex, "Error while preparing download for user {UserId}", requestedByUser);
                }

                throw;
            }
        }
    }
}

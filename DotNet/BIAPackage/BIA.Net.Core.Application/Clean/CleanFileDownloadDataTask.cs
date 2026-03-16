// <copyright file="CleanFileDownloadDataTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Clean
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.File;
    using BIA.Net.Core.Application.Job;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Hangfire task to clean expired file to download data by deleting the database entry and the file in the storage.
    /// </summary>
    [AutomaticRetry(Attempts = 0)]
    public sealed class CleanFileDownloadDataTask : BaseJob
    {
        private readonly IFileDownloaderService fileDownloaderService;
        private readonly IFileDownloadDataAppService fileDownloadDataAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanFileDownloadDataTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fileDownloaderService">The file downloader service.</param>
        /// <param name="fileDownloadDataAppService">The file download data app service.</param>
        public CleanFileDownloadDataTask(IConfiguration configuration, ILogger<CleanFileDownloadDataTask> logger, IFileDownloaderService fileDownloaderService, IFileDownloadDataAppService fileDownloadDataAppService)
            : base(configuration, logger)
        {
            this.fileDownloaderService = fileDownloaderService;
            this.fileDownloadDataAppService = fileDownloadDataAppService;
        }

        /// <inheritdoc/>
        protected override async Task RunMonitoredTask()
        {
            var fileDownloadDataToDelete = await this.fileDownloadDataAppService.GetAllAsync(
                filter: fd => fd.ExpiredAtDateTime.HasValue && fd.ExpiredAtDateTime.Value < DateTime.UtcNow,
                isReadOnlyMode: true);

            foreach (var fileDownloadData in fileDownloadDataToDelete)
            {
                try
                {
                    this.Logger.LogInformation("File to download with id {Id} is expired and will be deleted", fileDownloadData.Id);
                    await this.fileDownloaderService.RemoveFileToDownload(fileDownloadData);
                    this.Logger.LogInformation("File to download with id {Id} has been deleted", fileDownloadData.Id);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, "An error occurred while deleting file to download with id {Id}", fileDownloadData.Id);
                }
            }
        }
    }
}

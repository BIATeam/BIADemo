// <copyright file="CleanFileDownloadDataTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Clean
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.File.Mappers;
    using BIA.Net.Core.Domain.RepoContract;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Hangfire task to clean expired file to download data by deleting the database entry and the file in the storage.
    /// </summary>
    [AutomaticRetry(Attempts = 0)]
    public sealed class CleanFileDownloadDataTask : BaseJob
    {
        private readonly ITGenericRepository<FileDownloadData, Guid> fileDownloadDataRepository;
        private readonly IBiaFileDownloaderService fileDownloaderService;
        private readonly FileDownloadDataMapper fileDownloadDataMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanFileDownloadDataTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fileDownloadDataRepository">The file download data repository.</param>
        /// <param name="fileDownloaderService">The file downloader service.</param>
        public CleanFileDownloadDataTask(IConfiguration configuration, ILogger<CleanFileDownloadDataTask> logger, ITGenericRepository<FileDownloadData, Guid> fileDownloadDataRepository, IBiaFileDownloaderService fileDownloaderService, FileDownloadDataMapper fileDownloadDataMapper)
            : base(configuration, logger)
        {
            this.fileDownloadDataRepository = fileDownloadDataRepository;
            this.fileDownloaderService = fileDownloaderService;
            this.fileDownloadDataMapper = fileDownloadDataMapper;
        }

        /// <inheritdoc/>
        protected override async Task RunMonitoredTask()
        {
            var fileDownloadDataToDelete = await this.fileDownloadDataRepository.GetAllResultAsync(
                this.fileDownloadDataMapper.EntityToDto(),
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

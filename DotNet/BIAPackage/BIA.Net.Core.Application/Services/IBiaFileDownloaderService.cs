// <copyright file="IBiaFileDownloaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.File;

    /// <summary>
    /// Interface for the file downloader service that prepares file download and notifies the user when the file is ready to be downloaded.
    /// </summary>
    public interface IBiaFileDownloaderService
    {
        /// <summary>
        /// Prepares the file download by enqueuing a background job that will generate the file and notify the user when it's ready.
        /// </summary>
        /// <typeparam name="TBackgroundFileGeneratorService">Type of the background file generator service.</typeparam>
        /// <param name="requestedByUserId">The ID of the user who requested the download.</param>
        void PrepareDownload<TBackgroundFileGeneratorService>(int requestedByUserId)
            where TBackgroundFileGeneratorService : IBiaBackgroundFileGeneratorService;

        /// <summary>
        /// Job method that generates the file and notifies the user when it's ready. This method is executed in the background by Hangfire.
        /// </summary>
        /// <param name="generatorType">Type of the background file generator service.</param>
        /// <param name="requestedByUserId">The ID of the user who requested the download.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PrepareDownloadJobAsync(Type generatorType, int requestedByUserId);

        /// <summary>
        /// Generates a download token for the specified file and user. This token can be used to securely download the file without exposing the file path or other sensitive information.
        /// </summary>
        /// <param name="fileGuid">The unique identifier of the file.</param>
        /// <param name="requestedByUserId">The ID of the user who requested the download.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the download token.</returns>
        Task<string> GenerateDownloadToken(Guid fileGuid, int requestedByUserId);

        /// <summary>
        /// Gets the file download data for the specified file and download token. This method validates the download token and retrieves the file information needed for the download.
        /// </summary>
        /// <param name="fileGuid">The unique identifier of the file.</param>
        /// <param name="token">The download token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the file download data.</returns>
        Task<FileDownloadDataDto> GetFileDownloadData(Guid fileGuid, string token);
    }
}

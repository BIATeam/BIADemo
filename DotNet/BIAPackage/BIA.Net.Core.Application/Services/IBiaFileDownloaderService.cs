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
        /// Prepares the file download by executing the provided function to get the file download data, and then notifies the user when the file is ready to be downloaded.
        /// </summary>
        /// <param name="getFileDownloadDataTask">The task to retrieve the file download data.</param>
        /// <param name="requestedByUserId">The ID of the user who requested the download.</param>
        void PrepareDownload(Func<Task<FileDownloadDataDto>> getFileDownloadDataTask, int requestedByUserId);

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

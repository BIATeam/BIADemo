// <copyright file="IBiaFileDownloaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Interface for the file downloader service that prepares file download and notifies the user when the file is ready to be downloaded.
    /// </summary>
    public interface IBiaFileDownloaderService
    {
        /// <summary>
        /// Prepares a background file download by calling a method on an already-registered DI service.
        /// </summary>
        /// <typeparam name="TService">Type of the DI-registered service that exposes the generation method.</typeparam>
        /// <param name="requestedByUserId">ID of the user who requested the download.</param>
        /// <param name="generateFileExpression">Expression pointing to the generation method.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task PrepareBackgroundDownloadAsync<TService>(int requestedByUserId, Expression<Func<TService, Task<FileDownloadDataDto>>> generateFileExpression)
            where TService : class;

        /// <summary>
        /// Notifies the user that the file is ready to be downloaded by creating a notification and saving the file download data in the database.
        /// </summary>
        /// <param name="fileDownloadDataDto">The file download data transfer object.</param>
        /// <param name="requestedByUser">The user who requested the download.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task NotifyDownloadReadyAsync(FileDownloadDataDto fileDownloadDataDto, BaseEntityUser requestedByUser);

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

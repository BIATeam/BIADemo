// <copyright file="IFileDownloadTokenRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.File.Entities;

    /// <summary>
    /// Interface for the file download token repository.
    /// </summary>
    public interface IFileDownloadTokenRepository
    {
        /// <summary>
        /// Adds a file download token to the repository.
        /// </summary>
        /// <param name="fileDownloadToken">The file download token to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(FileDownloadToken fileDownloadToken);

        /// <summary>
        /// Removes the specified file download token asynchronously.
        /// </summary>
        /// <param name="fileDownloadToken">The file download token to remove.</param>
        /// <returns>A task that represents the asynchronous remove operation.</returns>
        Task RemoveAsync(FileDownloadToken fileDownloadToken);

        /// <summary>
        /// Gets a file download token by the specified file GUID and token asynchronously.
        /// </summary>
        /// <param name="fileGuid">The unique identifier of the file.</param>
        /// <param name="token">The download token.</param>
        /// <returns>A task that represents the asynchronous operation, with a value of the file download token if found.</returns>
        Task<FileDownloadToken> GetAsync(Guid fileGuid, string token);
    }
}
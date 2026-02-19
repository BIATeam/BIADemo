// <copyright file="IBiaBackgroundFileGeneratorService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.File;

    /// <summary>
    /// Interface for asynchronous file generators used in background jobs.
    /// </summary>
    public interface IBiaBackgroundFileGeneratorService
    {
        /// <summary>
        /// Generates a file asynchronously and returns the file download data.
        /// </summary>
        /// <returns>A task that returns the file download data.</returns>
        Task<FileDownloadDataDto> GenerateAsync();
    }
}

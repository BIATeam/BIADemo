// <copyright file="IExampleAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Example
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface defining the application service for examples in BIADemo.
    /// </summary>
    public interface IExampleAppService
    {
        /// <summary>
        /// Create a notification to inform the user that the download of a file is ready, with the file to download.
        /// </summary>
        /// <param name="requestedByUserId">The ID of the user who requested the download.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task NotifyDownloadReadyFileExample(int requestedByUserId);
    }
}
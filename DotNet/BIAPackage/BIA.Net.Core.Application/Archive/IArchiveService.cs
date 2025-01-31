// <copyright file="IArchiveService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Archive
{
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for archive service.
    /// </summary>
    public interface IArchiveService
    {
        /// <summary>
        /// Run the service.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task RunAsync();
    }
}

// <copyright file="IDatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Public interface for implementations of <see cref="IDatabaseHandlerRepository"/>.
    /// </summary>
    public interface IDatabaseHandlerRepository
    {
        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        /// <returns>A completed <see cref="Task"/>.</returns>
        Task Start();

        /// <summary>
        /// Stop the process of event handler.
        /// </summary>
        /// <returns>A completed <see cref="Task"/>.</returns>
        Task Stop();
    }
}
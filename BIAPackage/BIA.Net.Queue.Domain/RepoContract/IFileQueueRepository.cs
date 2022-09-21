// <copyright file="IFileQueueRepository.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using BIA.Net.Queue.Domain.Dto.FileQueue;
    using BIA.Net.Queue.Domain.Dto.Queue;

    /// <summary>
    /// Interface of FileQueueRepository
    /// </summary>
    public interface IFileQueueRepository
    {
        /// <summary>
        /// Configure the queues.
        /// </summary>
        /// <param name="queues">The list of <see cref="QueueDto"/>.</param>
        void Configure(IEnumerable<QueueDto> queues);

        /// <summary>
        /// Subscribe to recieve <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileQueueDto"/>.</param>
        /// <returns></returns>
        IDisposable Subscribe(IObserver<FileQueueDto> observer);

        /// <summary>
        /// Subscribe to recieve <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileQueueDto"/>.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>
        /// <returns></returns>
        IDisposable Subscribe(IObserver<FileQueueDto> observer, string user, string password);

        /// <summary>
        /// Send a new <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="file"> The file.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        bool SendFile(string endpoint, string queueName, FileQueueDto file);

        /// <summary>
        /// Send a new <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="file"> The file.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        bool SendFile(string endpoint, string queueName, FileQueueDto file, string user, string password);
    }
}

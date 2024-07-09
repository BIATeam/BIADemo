// <copyright file="IFileQueueRepository.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using BIA.Net.Queue.Domain.Dto.FileQueue;
    using BIA.Net.Queue.Domain.Dto.Queue;

    /// <summary>
    /// Interface of FileQueueRepository.
    /// </summary>
    public interface IFileQueueRepository
    {
        /// <summary>
        /// Configure the queues.
        /// </summary>
        /// <param name="topics">The list of <see cref="TopicDto"/>.</param>
        void Configure(IEnumerable<TopicDto> topics);

        /// <summary>
        /// Subscribe to receive <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileQueueDto"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An iDisposable.</returns>
        IDisposable Subscribe(IObserver<FileMessageDto> observer, CancellationToken cancellationToken);

        /// <summary>
        /// Subscribe to receive <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileQueueDto"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>

        /// <returns>An iDisposable.</returns>
        IDisposable Subscribe(IObserver<FileMessageDto> observer, CancellationToken cancellationToken, string user, string password);

        /// <summary>
        /// Send a new <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="topic">The global information to read a RabbitMQ Topic.</param>
        /// <param name="file"> The file.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        bool SendFile(TopicDto topic, FileMessageDto file);

        /// <summary>
        /// Send a new <see cref="FileQueueDto"/>.
        /// </summary>
        /// <param name="topic">The global information to read a RabbitMQ Topic.</param>
        /// <param name="file"> The file.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        bool SendFile(TopicDto topic, FileMessageDto file, string user, string password);
    }
}

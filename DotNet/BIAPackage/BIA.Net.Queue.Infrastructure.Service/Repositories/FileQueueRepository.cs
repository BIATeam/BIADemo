// <copyright file="FileQueueRepository.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using BIA.Net.Queue.Common.Observer;
    using BIA.Net.Queue.Domain.Dto.FileQueue;
    using BIA.Net.Queue.Domain.Dto.Queue;
    using BIA.Net.Queue.Domain.RepoContract;
    using BIA.Net.Queue.Infrastructure.Service.Helpers;

    /// <summary>
    /// Repository for file queuing.
    /// </summary>
    public sealed class FileQueueRepository : IObservable<FileMessageDto>, IFileQueueRepository
    {
        private readonly List<IObserver<FileMessageDto>> observers;
        private IEnumerable<TopicDto> topics;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileQueueRepository"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint address of queue Server.</param>
        /// <param name="queueName">The queue name listen.</param>
        public FileQueueRepository()
        {
            this.observers = new List<IObserver<FileMessageDto>>();
        }

        /// <inheritdoc />
        public void Configure(IEnumerable<TopicDto> topics)
        {
            if (topics == null)
            {
                throw new ArgumentException("queues cannot be null");
            }

            this.topics = topics;
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<FileMessageDto> observer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<FileMessageDto> observer, CancellationToken cancellationToken)
        {
            // Check whether observer is already registered. If not, add it
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);

                foreach (TopicDto topic in this.topics)
                {
                    RabbitMQHelper.ReceiveMessageAsync<FileMessageDto>(topic, observer.OnNext, cancellationToken);
                }
            }

            return new Unsubscriber<FileMessageDto>(this.observers, observer);
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<FileMessageDto> observer, CancellationToken cancellationToken, string user, string password)
        {
            // Check whether observer is already registered. If not, add it
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);

                foreach (TopicDto topic in this.topics)
                {
                    RabbitMQHelper.ReceiveMessageAsync<FileMessageDto>(topic, observer.OnNext, cancellationToken, user, password);
                }
            }

            return new Unsubscriber<FileMessageDto>(this.observers, observer);
        }

        /// <inheritdoc />
        public bool SendFile(TopicDto topic, FileMessageDto file)
        {
             return RabbitMQHelper.SendMessage(topic, file);
        }

        /// <inheritdoc />
        public bool SendFile(TopicDto topic, FileMessageDto file, string user, string password)
        {
            return RabbitMQHelper.SendMessage(topic, file, user, password);
        }
    }
}
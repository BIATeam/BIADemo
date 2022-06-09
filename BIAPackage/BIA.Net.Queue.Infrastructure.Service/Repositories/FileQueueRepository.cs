// <copyright file="FileQueueRepository.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Infrastructure.Service.Repositories
{
    using BIA.Net.Queue.Common.Observer;
    using BIA.Net.Queue.Domain.Dto.FileQueue;
    using BIA.Net.Queue.Domain.Dto.Queue;
    using BIA.Net.Queue.Domain.RepoContract;
    using BIA.Net.Queue.Infrastructure.Service.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Repository for file queuing.
    /// </summary>
    public class FileQueueRepository : IObservable<FileQueueDto>, IFileQueueRepository
    {
        private List<IObserver<FileQueueDto>> observers;
        private readonly QueuesHelper queuesHelper;
        private IEnumerable<QueueDto> queues;

        Dictionary<string, CancellationTokenSource> recieverLaunched;

        /// <summary>
        /// Instanciate a <see cref="FileQueueRepository"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint address of queue Server.</param>
        /// <param name="queueName">The queue name listen.</param>
        public FileQueueRepository()
        {
            
            this.observers = new List<IObserver<FileQueueDto>>();

            queuesHelper = new QueuesHelper();
            recieverLaunched = new Dictionary<string, CancellationTokenSource>();
        }

        /// <inheritdoc />
        public void Configure(IEnumerable<QueueDto> queues)
        {
            if (queues == null)
            {
                throw new ArgumentException("queues cannot be null");
            }

            this.queues = queues;
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<FileQueueDto> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                foreach (QueueDto queue in queues)
                {
                    queuesHelper.ReceiveMessage<FileQueueDto>(queue.Endpoint, queue.QueueName, observer.OnNext);
                }
            }
            return new Unsubscriber<FileQueueDto>(observers, observer);
        }

        /// <inheritdoc />
        public bool SendFile(string endpoint, string queueName, FileQueueDto file)
        {
             return queuesHelper.SendMessage(endpoint, queueName, file);
        }

        /// <summary>
        /// Destructor of <see cref="FileQueueRepository"/>.
        /// </summary>
        ~FileQueueRepository()
        {
            queuesHelper.Dispose();
        }
    }
}

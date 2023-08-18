// <copyright file="DataBaseHandlerService.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    public class DataBaseHandlerService : IHostedService, IDisposable
    {
        private readonly List<DatabaseHandlerRepository> databaseHandlerRepositories;

        public DataBaseHandlerService(List<DatabaseHandlerRepository> databaseHandlerRepositories)
        {
            this.databaseHandlerRepositories = databaseHandlerRepositories;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var handlerRepositorie in this.databaseHandlerRepositories)
            {
                handlerRepositorie.Start();
            }

            return Task.CompletedTask;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task StopAsync(CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (var handlerRepositorie in this.databaseHandlerRepositories)
            {
                handlerRepositorie.Stop();
            }
        }

        public virtual void Dispose()
        {
        }
    }
}

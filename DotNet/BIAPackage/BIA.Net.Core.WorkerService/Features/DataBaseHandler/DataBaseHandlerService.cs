using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    public class DataBaseHandlerService : IHostedService, IDisposable
    {
        private readonly List<DatabaseHandlerRepository> DatabaseHandlerRepositories;

        public DataBaseHandlerService(List<DatabaseHandlerRepository> DatabaseHandlerRepositories)
        {
            this.DatabaseHandlerRepositories = DatabaseHandlerRepositories;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var handlerRepositorie in DatabaseHandlerRepositories)
            {
                handlerRepositorie.Start();
            }
            return Task.CompletedTask;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task StopAsync(CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (var handlerRepositorie in DatabaseHandlerRepositories)
            {
                handlerRepositorie.Stop();
            }
        }

        public virtual void Dispose()
        {

        }
    }
}

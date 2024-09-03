// <copyright file="DataBaseHandlerService.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// DataBaseHandler Service.
    /// </summary>
    public class DataBaseHandlerService : IHostedService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<DataBaseHandlerService> logger;

        /// <summary>
        /// The database handler repositories.
        /// </summary>
        private readonly List<IDatabaseHandlerRepository> databaseHandlerRepositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseHandlerService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="databaseHandlerRepositories">The database handler repositories.</param>
        public DataBaseHandlerService(ILogger<DataBaseHandlerService> logger, IEnumerable<IDatabaseHandlerRepository> databaseHandlerRepositories)
        {
            this.logger = logger;
            this.databaseHandlerRepositories = databaseHandlerRepositories.ToList();
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        protected ILogger<DataBaseHandlerService> Logger => this.logger;

        /// <summary>
        /// The database handler repositories.
        /// </summary>
        protected List<IDatabaseHandlerRepository> DatabaseHandlerRepositories => this.databaseHandlerRepositories;

        /// <inheritdoc cref="IHostedService.StartAsync"/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(DataBaseHandlerService)}.{nameof(this.StartAsync)}");
            this.logger.LogInformation($"DatabaseHandlerRepositories.Count: {this.DatabaseHandlerRepositories.Count}");

            foreach (var handlerRepositorie in this.DatabaseHandlerRepositories)
            {
                await handlerRepositorie.Start();
            }
        }

        /// <inheritdoc cref="IHostedService.StopAsync"/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(DataBaseHandlerService)}.{nameof(this.StopAsync)}");
            this.logger.LogInformation($"DatabaseHandlerRepositories.Count: {this.databaseHandlerRepositories.Count}");

            foreach (var handlerRepositorie in this.databaseHandlerRepositories)
            {
                await handlerRepositorie.Stop();
            }
        }
    }
}

// <copyright file="DataBaseHandlerService.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// DataBaseHandler Service.
    /// </summary>
    public class DataBaseHandlerService : IHostedService
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

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
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="databaseHandlerRepositories">The database handler repositories.</param>
        public DataBaseHandlerService(IServiceProvider serviceProvider, List<IDatabaseHandlerRepository> databaseHandlerRepositories)
        {
            this.serviceProvider = serviceProvider;
            this.logger = this.serviceProvider.GetService<ILogger<DataBaseHandlerService>>();
            this.databaseHandlerRepositories = databaseHandlerRepositories;
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        protected IServiceProvider ServiceProvider => this.serviceProvider;

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        protected ILogger<DataBaseHandlerService> Logger => this.logger;

        /// <inheritdoc cref="IHostedService.StartAsync"/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(DataBaseHandlerService)}.{nameof(this.StartAsync)}");
            string message = $"DatabaseHandlerRepositories.Count: {this.databaseHandlerRepositories.Count}";
            this.logger.LogInformation(message);

            foreach (var handlerRepositorie in this.databaseHandlerRepositories)
            {
                handlerRepositorie.Start(this.serviceProvider);
            }

            return Task.CompletedTask;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <inheritdoc cref="IHostedService.StopAsync"/>
        public async Task StopAsync(CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.logger.LogInformation($"{nameof(DataBaseHandlerService)}.{nameof(this.StopAsync)}");
            string message = $"databaseHandlerRepositories.Count: {this.databaseHandlerRepositories.Count}";
            this.logger.LogInformation(message);

            foreach (var handlerRepositorie in this.databaseHandlerRepositories)
            {
                handlerRepositorie.Stop();
            }
        }
    }
}

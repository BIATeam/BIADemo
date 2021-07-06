// <copyright file="DeployDBService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.DeployDB
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;

    /// <summary>
    /// Service to deploy DB DataContext.
    /// </summary>
    internal sealed class DeployDBService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime appLifetime;

        private readonly DataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeployDBService"/> class.
        /// </summary>
        /// <param name="logger">the logger.</param>
        /// <param name="appLifetime">the app life time.</param>
        /// <param name="dataContext">the data context.</param>
        public DeployDBService(
            ILogger<DeployDBService> logger,
            IHostApplicationLifetime appLifetime,
            DataContext dataContext)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.dataContext = dataContext;
        }

        /// <summary>
        /// The Deployement of the db.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Task.CompletedTask.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            this.appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        this.logger.LogInformation("Start Migration!");

                        this.dataContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Unhandled exception!");
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        this.appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// The stop.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Task.CompletedTask.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.dataContext.Dispose();
            return Task.CompletedTask;
        }
    }
}

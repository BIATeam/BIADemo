// <copyright file="DeployDBService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.DeployDB
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;
        private readonly DataContext dataContext;
        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeployDBService" /> class.
        /// </summary>
        /// <param name="logger">the logger.</param>
        /// <param name="appLifetime">the app life time.</param>
        /// <param name="dataContext">the data context.</param>
        /// <param name="configuration">The configuration.</param>
        public DeployDBService(
            ILogger<DeployDBService> logger,
            IHostApplicationLifetime appLifetime,
            DataContext dataContext,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.dataContext = dataContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// The Deployement of the db.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Task.CompletedTask.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            string message = $"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}";
            this.logger.LogDebug(message);

            this.appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        this.logger.LogInformation("Start Migration!");

                        string message = $"ConnectionString: {this.dataContext.Database.GetDbConnection().ConnectionString}";
                        this.logger.LogInformation(message);

                        // Database Migrate CommandTimeout
                        string confCommandTimeout = "DatabaseMigrate:CommandTimeout";
                        int timeout = int.Parse(this.configuration["DatabaseMigrate:CommandTimeout"]);

                        message = $"{confCommandTimeout}: {timeout} minutes";
                        this.logger.LogInformation(message);
                        this.dataContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeout));

                        //this.dataContext.Database.Migrate();

                        this.CleanDistCacheAsync().Wait();
                    }
                    catch (Exception ex)
                    {
                        this.exception = ex;
                        this.logger.LogError(ex, "Unhandled exception!");
                        throw new BIA.Net.Core.Common.Exceptions.SystemException("Unhandled exception!", ex);
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

            if (this.exception != default)
            {
                return Task.FromException(this.exception);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Cleans the dist cache asynchronous.
        /// </summary>
        private async Task CleanDistCacheAsync()
        {
            try
            {
                this.dataContext.DistCache.RemoveRange(this.dataContext.DistCache);
                int nb = await this.dataContext.CommitAsync();
                var message = $"DistCache cleaned (nb removed: {nb})";
                this.logger.LogInformation(message);
            }
            catch (Exception ex)
            {
                string methodName = nameof(this.CleanDistCacheAsync);
                this.logger.LogWarning(exception: ex, message: methodName, args: default);
            }
        }
    }
}

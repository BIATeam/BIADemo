// BIADemo only
// <copyright file="Worker.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;

    // Begin BIADemo
    using System.Security.Cryptography;

    // End BIADemo
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.WorkerService;
    using Hangfire;

    // Begin BIADemo
    using Hangfire.States;

    // End BIADemo
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Job;

    /// <summary>
    /// Worker class.
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<Worker> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public Worker(
            IConfiguration configuration,
            ILogger<Worker> logger)
        {
            this.Configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": BIADemo Server started.");
            while (!stoppingToken.IsCancellationRequested)
            {

                var client = new BackgroundJobClient();
                client.Create<ExampleTask>(x => x.Run(), new EnqueuedState());

                await Task.Delay(600000);
            }
        }
    }
}
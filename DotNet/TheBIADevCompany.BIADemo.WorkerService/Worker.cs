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
    using TheBIADevCompany.BIADemo.WorkerService.Job;

    /// <summary>
    /// Worker class.
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Worker(IConfiguration configuration)
        {
            this.Configuration = configuration;
            string projectName = configuration["Project:Name"];

            RecuringJobsHelper.CleanHangfireServerQueue();

            RecurringJob.AddOrUpdate<WakeUpTask>($"{projectName}.{typeof(WakeUpTask).Name}", t => t.Run(), configuration["Tasks:WakeUp:CRON"]);
            RecurringJob.AddOrUpdate<SynchronizeUserTask>($"{projectName}.{typeof(SynchronizeUserTask).Name}", t => t.Run(), configuration["Tasks:SynchronizeUser:CRON"]);
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
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": BIADemo Server started.");

            // Begin BIADemo

            // for a custom scheduling
            while (true)
            {
                var client = new BackgroundJobClient();
                client.Create<ExampleTask>(x => x.Run(), new EnqueuedState());
                await Task.Delay(RandomNumberGenerator.GetInt32(1800000, 7200000)); // Random delay beetween 1 800 000 ms (=30 min) and 7 200 000 ms (=2 h)
            }

            // End BIADemo
        }
    }
}
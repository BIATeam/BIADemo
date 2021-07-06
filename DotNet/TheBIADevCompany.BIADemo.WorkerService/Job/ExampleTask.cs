// BIADemo only
// <copyright file="ExampleTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.WorkerService.Job
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Example of task lanched manualy with hangfire.
    /// </summary>
    [AutomaticRetry(Attempts = 0, LogEvents = true)]
    internal class ExampleTask : BaseJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userService">The user app service.</param>
        /// <param name="logger">logger.</param>
        public ExampleTask(IConfiguration configuration, ILogger<SynchronizeUserTask> logger)
            : base(configuration, logger)
        {
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task RunMonitoredTask()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Hello from the job ExampleTask.");
        }
    }
}

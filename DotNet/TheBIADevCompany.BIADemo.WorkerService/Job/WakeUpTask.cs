// <copyright file="WakeUpTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Job
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common.Helpers;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Task to wake up.
    /// </summary>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    internal class WakeUpTask : BaseJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WakeUpTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="roleAppService">The role app service.</param>
        /// <param name="logger">logger.</param>
        public WakeUpTask(IConfiguration configuration, ILogger<WakeUpTask> logger)
            : base(configuration, logger)
        {
        }

        /// <summary>
        /// Call all urls in Tasks:WakeUp:Url split by '|'.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            string[] urlToWakeUp = this.Configuration["Tasks:WakeUp:Url"].Split("|");
            var requestTasks = new List<Task>();
            foreach (var url in urlToWakeUp)
            {
                requestTasks.Add(RequestHelper.GetAsync(url, null));
            }

            await Task.WhenAll(requestTasks);
        }
    }
}

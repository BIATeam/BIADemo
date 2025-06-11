// <copyright file="WakeUpTask.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Job
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.RepoContract;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Task to wake up.
    /// </summary>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class WakeUpTask : BaseJob
    {
        /// <summary>
        /// the front webiApi repository.
        /// </summary>
        private readonly IWakeUpWebApps wakeUpWebApps;

        /// <summary>
        /// Initializes a new instance of the <see cref="WakeUpTask" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">logger.</param>
        /// <param name="webApiRepository">the front webiApi repository.</param>
        /// <param name="appRepository">The application repository.</param>
        /// <param name="wakeUpWebApps">The WebApps to wake up.</param>
        public WakeUpTask(IConfiguration configuration, ILogger<WakeUpTask> logger, IWakeUpWebApps wakeUpWebApps)
            : base(configuration, logger)
        {
            this.wakeUpWebApps = wakeUpWebApps;
        }

        /// <summary>
        /// Call all urls in Tasks:WakeUp:Url split by '|'.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            List<Task<(bool IsSuccessStatusCode, string ReasonPhrase)>> wakeUpWebAppTasks = this.wakeUpWebApps.WakeUp();

            await Task.WhenAll(wakeUpWebAppTasks);

            StringBuilder bld = new();

            foreach (var (isSuccessStatusCode, reasonPhrase) in wakeUpWebAppTasks.Select(t => t.Result))
            {
                if (!isSuccessStatusCode)
                {
                    bld.Append("Error when wakeUp : " + reasonPhrase + ". ");
                }
            }

            string error = bld.ToString();

            if (!string.IsNullOrEmpty(error))
            {
                throw new JobException(error);
            }
        }
    }
}

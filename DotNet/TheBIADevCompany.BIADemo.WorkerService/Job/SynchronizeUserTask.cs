// <copyright file="SynchronizeUserTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Job
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.User;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    internal class SynchronizeUserTask : BaseJob
    {
        private readonly IUserAppService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeUserTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userService">The user app service.</param>
        /// <param name="logger">logger.</param>
        public SynchronizeUserTask(IConfiguration configuration, IUserAppService userService, ILogger<SynchronizeUserTask> logger)
            : base(configuration, logger)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            await this.userService.SynchronizeWithADAsync(true);
        }
    }
}

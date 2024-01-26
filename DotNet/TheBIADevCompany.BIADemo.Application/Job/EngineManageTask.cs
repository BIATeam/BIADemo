// BIADemo only
// <copyright file="EngineManageTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Plane;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class EngineManageTask : BaseJob
    {
        private readonly IEngineAppService engineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineManageTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="engineService">The engine service.</param>
        /// <param name="logger">The logger.</param>
        public EngineManageTask(IConfiguration configuration, IEngineAppService engineService, ILogger<EngineManageTask> logger)
            : base(configuration, logger)
        {
            this.engineService = engineService;
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            await this.engineService.CheckToBeMaintainedAsync();
        }
    }
}

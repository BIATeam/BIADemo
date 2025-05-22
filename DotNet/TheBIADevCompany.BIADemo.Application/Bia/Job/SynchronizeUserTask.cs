// <copyright file="SynchronizeUserTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.Job
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common.Configuration;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Bia.User;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class SynchronizeUserTask : BaseJob
    {
        private readonly IUserAppService userService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection biaNetSection;

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
            this.biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            if (this.biaNetSection?.Authentication?.Keycloak?.IsActive == true)
            {
                await this.userService.SynchronizeWithIdpAsync();
            }
            else
            {
                await this.userService.SynchronizeWithADAsync(true);
            }
        }
    }
}

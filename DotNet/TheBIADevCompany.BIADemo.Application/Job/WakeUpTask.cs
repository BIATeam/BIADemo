// <copyright file="WakeUpTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Task to wake up.
    /// </summary>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class WakeUpTask : BaseJob
    {
        /// <summary>
        /// the front webiApi repository.
        /// </summary>
        private readonly IBIADemoWebApiRepository webApiRepository;

        /// <summary>
        /// the front app repository.
        /// </summary>
        private readonly IBIADemoAppRepository appRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WakeUpTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="roleAppService">The role app service.</param>
        /// <param name="logger">logger.</param>
        /// <param name="webApiRepository">the front webiApi repository.</param>
        public WakeUpTask(IConfiguration configuration, ILogger<WakeUpTask> logger, IBIADemoWebApiRepository webApiRepository, IBIADemoAppRepository appRepository)
            : base(configuration, logger)
        {
            this.webApiRepository = webApiRepository;
            this.appRepository = appRepository;
        }

        /// <summary>
        /// Call all urls in Tasks:WakeUp:Url split by '|'.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            Task<(bool IsSuccessStatusCode, string ReasonPhrase)> wakeUpWebApi = this.webApiRepository.WakeUp();
            Task<(bool IsSuccessStatusCode, string ReasonPhrase)> wakeUpApp = this.appRepository.WakeUp();
            await Task.WhenAll(wakeUpWebApi);

            string error = string.Empty;

            if (!wakeUpWebApi.Result.IsSuccessStatusCode)
            {
                error = error + "Error when wakeUp WebApi : " + wakeUpWebApi.Result.ReasonPhrase + ". ";
            }

            if (!wakeUpApp.Result.IsSuccessStatusCode)
            {
                error = error + "Error when wakeUp App : " + wakeUpApp.Result.ReasonPhrase + ". ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                throw new JobException(error);
            }
        }
    }
}

// <copyright file="CleanTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Clean
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Clean task.
    /// </summary>
    public class CleanTask : BaseJob
    {
        private readonly IReadOnlyList<ICleanService> cleanServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cleanServices">The clean services.</param>
        public CleanTask(IConfiguration configuration, ILogger<CleanTask> logger, IEnumerable<ICleanService> cleanServices)
            : base(configuration, logger)
        {
            this.cleanServices = cleanServices.ToList();
        }

        /// <inheritdoc/>
        protected override async Task RunMonitoredTask()
        {
            this.Logger.LogInformation("Start Clean Task");
            foreach (var cleanService in this.cleanServices)
            {
                await cleanService.RunAsync();
            }

            this.Logger.LogInformation("End Clean Task");
        }
    }
}

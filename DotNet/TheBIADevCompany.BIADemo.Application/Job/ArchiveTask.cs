// <copyright file="ArchiveTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common.Configuration;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Archive task.
    /// </summary>
    [AutomaticRetry(Attempts = 0, LogEvents = true)]
    public class ArchiveTask : BaseJob
    {
        private readonly BiaNetSection biaNetSection;
        private readonly List<IArchiveService> archiveServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="archiveServices">The injected list of <see cref="IArchiveService"/>.</param>
        public ArchiveTask(IConfiguration configuration, ILogger<ArchiveTask> logger, IEnumerable<IArchiveService> archiveServices)
            : base(configuration, logger)
        {
            this.biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(this.biaNetSection);

            this.archiveServices = archiveServices.ToList();
        }

        /// <inheritdoc/>
        protected override async Task RunMonitoredTask()
        {
            try
            {
                this.Logger.LogInformation("Start Archive Task");

                if (this.biaNetSection.WorkerFeatures.Archive is null)
                {
                    this.Logger.LogWarning("Unable to find archive configuration.");
                    return;
                }

                if (!this.biaNetSection.WorkerFeatures.Archive.IsActive)
                {
                    return;
                }

                if (this.archiveServices.Count == 0)
                {
                    this.Logger.LogWarning("No archive service registered");
                    return;
                }

                foreach (var archiveService in this.archiveServices)
                {
                    await archiveService.RunAsync();
                }
            }
            finally
            {
                this.Logger.LogInformation("End Archive Task");
            }
        }
    }
}

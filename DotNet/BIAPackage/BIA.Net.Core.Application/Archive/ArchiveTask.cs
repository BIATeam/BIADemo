// <copyright file="ArchiveTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Archive
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Archive task.
    /// </summary>
    public sealed class ArchiveTask : BaseJob
    {
        private readonly ArchiveConfiguration archiveConfiguration;
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
            var biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(biaNetSection);
            this.archiveConfiguration = biaNetSection.WorkerFeatures?.Archive;

            this.archiveServices = archiveServices.ToList();
        }

        /// <inheritdoc/>
        protected override async Task RunMonitoredTask()
        {
            try
            {
                this.Logger.LogInformation("Start Archive Task");

                if (this.archiveConfiguration is null)
                {
                    this.Logger.LogWarning("Unable to find archive configuration");
                    return;
                }

                if (!this.archiveConfiguration.IsActive)
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

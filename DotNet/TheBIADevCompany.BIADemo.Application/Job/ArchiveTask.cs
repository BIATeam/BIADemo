namespace BIA.Net.Core.Application.Job
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class ArchiveTask : BaseJob
    {
        private readonly List<IArchiveService> archiveServices;

        public ArchiveTask(IConfiguration configuration, ILogger<ArchiveTask> logger, IEnumerable<IArchiveService> archiveServices) : base(configuration, logger)
        {
            this.archiveServices = archiveServices.ToList();
        }

        protected override async Task RunMonitoredTask()
        {
            try
            {
                if (this.archiveServices.Count == 0)
                {
                    Logger.Log(LogLevel.Warning, "No archive service registered");
                    return;
                }

                Logger.Log(LogLevel.Information, "Start Archive Task");
                await Task.WhenAll(archiveServices.Select(x => x.RunAsync()));
            }
            finally
            {
                Logger.Log(LogLevel.Information, "End Archive Task");
            }
        }
    }
}

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
        private readonly IReadOnlyList<IEntityArchiveTask> entityArchiveTasks;

        public ArchiveTask(IConfiguration configuration, ILogger<ArchiveTask> logger, IEnumerable<IEntityArchiveTask> entityArchiveTasks) : base(configuration, logger)
        {
            this.entityArchiveTasks = entityArchiveTasks.ToList();
        }

        protected override async Task RunMonitoredTask()
        {
            Logger.Log(LogLevel.Information, "Start Archive Task");
            await Task.WhenAll(entityArchiveTasks.Select(x => x.Run()));
            Logger.Log(LogLevel.Information, "End Archive Task");
        }
    }
}

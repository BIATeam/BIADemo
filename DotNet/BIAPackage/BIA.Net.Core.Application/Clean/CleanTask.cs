namespace BIA.Net.Core.Application.Clean
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class CleanTask : BaseJob
    {
        private readonly IReadOnlyList<ICleanService> cleanServices;

        public CleanTask(IConfiguration configuration, ILogger<CleanTask> logger, IEnumerable<ICleanService> cleanServices) : base(configuration, logger)
        {
            this.cleanServices = cleanServices.ToList();
        }

        protected override async Task RunMonitoredTask()
        {
            foreach(var cleanService in cleanServices)
            {
                await cleanService.RunAsync();
            }
        }
    }
}

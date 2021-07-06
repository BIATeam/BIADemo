

namespace BIA.Net.Core.WorkerService.Features
{
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using BIA.Net.Core.WorkerService.Features.ClientForHub;
    using Microsoft.Extensions.Configuration;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.WorkerService.Features.HangfireServer;

    public class WorkerFeaturesServiceOptions
    {
        public IConfiguration Configuration { get; set; }
        public DistributedCacheConfiguration DistributedCache { get; set; }
        public ClientForHubOptions ClientForHub { get; set; }
        public DatabaseHandlerOptions DatabaseHandler { get; set; }
        public HangfireServerOptions HangfireServer { get; set; }

        public WorkerFeaturesServiceOptions()
        {
            DistributedCache = new DistributedCacheConfiguration();
            ClientForHub = new ClientForHubOptions();
            DatabaseHandler = new DatabaseHandlerOptions();
            HangfireServer = new HangfireServerOptions();
        }

    }
}

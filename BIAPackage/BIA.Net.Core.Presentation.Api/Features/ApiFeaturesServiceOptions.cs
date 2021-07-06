using BIA.Net.Core.Common.Configuration;
using BIA.Net.Core.Common.Configuration.CommonFeature;
using BIA.Net.Core.Presentation.Api.Features.DelegateJobToWorker;
using BIA.Net.Core.Presentation.Api.Features.Swagger;
using BIA.Net.Core.Presentation.Common.Features.HubForClients;
using Microsoft.Extensions.Configuration;

namespace BIA.Net.Core.WorkerService.Features
{
    public class ApiFeaturesServiceOptions
    {
        public IConfiguration Configuration { get; set; }
        public DistributedCacheConfiguration DistributedCache { get; private set; }
        public SwaggerOptions Swagger { get; private set; }
        public HubForClientsOptions HubForClients { get; private set; }
        public DelegateJobToWorkerOptions DelegateJobToWorker { get; private set; }

        public ApiFeaturesServiceOptions()
        {
            DistributedCache = new DistributedCacheConfiguration();
            Swagger = new SwaggerOptions();
            HubForClients = new HubForClientsOptions();
            DelegateJobToWorker = new DelegateJobToWorkerOptions();
        }
    }
}

using BIA.Net.Core.Common.Configuration.CommonFeature;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    public class WorkerFeatures
    {
        /// <summary>
        /// Gets or sets the Distributed Cache feature configuration
        /// </summary>
        public DistributedCacheConfiguration DistributedCache { get; set; }

        /// <summary>
        /// Gets or sets the DatabaseHandler feature configuration.
        /// </summary>
        public DatabaseHandlerConfiguration DatabaseHandler { get; set; }

        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public ClientForHubConfiguration ClientForHub { get; set; }

        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public HubForClientsConfiguration HubForClients { get; set; }

        /// <summary>
        /// Gets or sets the Hangfire Server feature configuration
        /// </summary>
        public HangfireServerConfiguration HangfireServer { get; set; }
    }
}

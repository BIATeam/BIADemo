using BIA.Net.Core.Common.Configuration.CommonFeature;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.ApiFeature
{
    public class ApiFeatures
    {
        /// <summary>
        /// Gets or sets the Distributed Cache feature configuration
        /// </summary>
        public DistributedCacheConfiguration DistributedCache { get; set; }

        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public Swagger Swagger { get; set; }

        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public HubForClientsConfiguration HubForClients { get; set; }
        /// <summary>
        /// Gets or sets the DelegateJobToWorker feature configuration.
        /// </summary>
        public DelegateJobToWorker DelegateJobToWorker { get; set; }
    }
}

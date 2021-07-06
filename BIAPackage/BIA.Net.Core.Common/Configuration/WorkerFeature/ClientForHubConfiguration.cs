using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    public class ClientForHubConfiguration
    {
        /// <summary>
        /// Boolean to activate the feature HubForClients.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Url to join the SignalR Hub.
        /// </summary>
        public string SignalRUrl { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    /// <summary>
    /// Api Feature Hub for Client Coniguration.
    /// </summary>
    public class HubForClientsConfiguration
    {
        /// <summary>
        /// Boolean to activate the feature HubForClients.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Connexion string to join the redis server.
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Filter redis Chanel.
        /// </summary>
        public string RedisChannelPrefix { get; set; }


    }
}

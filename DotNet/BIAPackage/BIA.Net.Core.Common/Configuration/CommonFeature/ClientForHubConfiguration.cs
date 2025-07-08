// <copyright file="ClientForHubConfiguration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    /// <summary>
    /// ClientForHubConfiguration.
    /// </summary>
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

        /// <summary>
        /// Connection string for redis server.
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Channel prefix of redis messages.
        /// </summary>
        public string RedisChannelPrefix { get; set; }
    }
}

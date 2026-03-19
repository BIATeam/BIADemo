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
        /// Gets or sets a value indicating whether [use valkey].
        /// </summary>
        public bool UseValkey { get; set; }

        /// <summary>
        /// Url to join the SignalR Hub.
        /// </summary>
        public string SignalRUrl { get; set; }

        /// <summary>
        /// Connection string for redis server.
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Host for redis server.
        /// </summary>
        public string RedisHost { get; set; }

        /// <summary>
        /// Port for redis server.
        /// </summary>
        public int RedisPort { get; set; } = 6379;

        /// <summary>
        /// Channel prefix of redis messages.
        /// </summary>
        public string RedisChannelPrefix { get; set; }
    }
}

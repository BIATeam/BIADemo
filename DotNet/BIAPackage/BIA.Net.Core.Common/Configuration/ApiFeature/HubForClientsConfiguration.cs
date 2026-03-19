// <copyright file="HubForClientsConfiguration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.ApiFeature
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
        /// Gets or sets a value indicating whether [use valkey].
        /// </summary>
        public bool UseValkey { get; set; }

        /// <summary>
        /// Connexion string to join the redis server.
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Host to join the redis server.
        /// </summary>
        public string RedisHost { get; set; }

        /// <summary>
        /// Port to join the redis server.
        /// </summary>
        public int RedisPort { get; set; } = 6379;

        /// <summary>
        /// Filter redis Chanel.
        /// </summary>
        public string RedisChannelPrefix { get; set; }
    }
}

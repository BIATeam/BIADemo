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
        /// Connexion string to join the redis server.
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Filter redis Chanel.
        /// </summary>
        public string RedisChannelPrefix { get; set; }

        /// <summary>
        /// Enable TLS for Redis/Valkey connection.
        /// </summary>
        public bool RedisUseTls { get; set; }

        /// <summary>
        /// Optional SNI host name to use for TLS (e.g., AWS Valkey serverless endpoint).
        /// </summary>
        public string RedisSslHost { get; set; }

        /// <summary>
        /// If true, skip server certificate validation (not recommended for production).
        /// </summary>
        public bool RedisSkipCertificateValidation { get; set; }
    }
}

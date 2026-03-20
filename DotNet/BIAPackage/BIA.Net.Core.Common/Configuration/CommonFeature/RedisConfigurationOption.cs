// <copyright file="RedisConfigurationOption.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    /// <summary>
    /// The options relevant to a set of redis connections.
    /// </summary>
    public class RedisConfigurationOption
    {
        /// <summary>
        /// The endpoints defined for this configuration.
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Indicates whether the connection should be encrypted.
        /// </summary>
        public bool UseSsl { get; set; } = true;

        /// <summary>
        /// Gets or sets whether connect/configuration timeouts should be explicitly notified via a TimeoutException.
        /// </summary>
        public bool AbortOnConnectFail { get; set; } = false;

        /// <summary>
        /// A Boolean value that specifies whether the certificate revocation list is checked during authentication.
        /// </summary>
        public bool CheckCertificateRevocation { get; set; } = true;

        /// <summary>
        /// Whether exceptions include identifiable details (key names, additional .Data annotations).
        /// </summary>
        public bool IncludeDetailInExceptions { get; set; } = false;

        /// <summary>
        /// Automatically encodes and decodes channels.
        /// </summary>
        public string ChannelPrefix { get; set; }
    }
}

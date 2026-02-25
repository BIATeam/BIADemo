// <copyright file="HybridCacheConfiguration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    /// <summary>
    /// HybridCache Configuration.
    /// </summary>
    public class HybridCacheConfiguration
    {
        /// <summary>
        /// Gets or sets the expiration seconds.
        /// </summary>
        public int ExpirationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the local cache expiration seconds.
        /// </summary>
        public int LocalCacheExpirationSeconds { get; set; }
    }
}

// <copyright file="DistCache.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.DistCache.Entities
{
    using System;

    /// <summary>
    /// DistCache.
    /// </summary>
    public partial class DistCache
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public byte[] Value { get; set; }

        /// <summary>
        /// Gets or sets the expires at time.
        /// </summary>
        /// <value>
        /// The expires at time.
        /// </value>
        public DateTimeOffset ExpiresAtTime { get; set; }

        /// <summary>
        /// Gets or sets the sliding expiration in seconds.
        /// </summary>
        /// <value>
        /// The sliding expiration in seconds.
        /// </value>
        public long? SlidingExpirationInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the absolute expiration.
        /// </summary>
        /// <value>
        /// The absolute expiration.
        /// </value>
        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }
}

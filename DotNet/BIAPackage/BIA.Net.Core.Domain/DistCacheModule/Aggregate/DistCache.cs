// <copyright file="DistCache.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.DistCacheModule.Aggregate
{
    using System;
    using System.Collections.Generic;

    public partial class DistCache
    {
        public string Id { get; set; }

        public byte[] Value { get; set; }

        public DateTimeOffset ExpiresAtTime { get; set; }

        public long? SlidingExpirationInSeconds { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }
}

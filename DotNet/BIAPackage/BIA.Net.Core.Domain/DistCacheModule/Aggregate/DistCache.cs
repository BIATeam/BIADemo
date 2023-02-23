using System;
using System.Collections.Generic;

namespace BIA.Net.Core.Domain.DistCacheModule.Aggregate
{
    public partial class DistCache
    {
        public string Id { get; set; }
        public byte[] Value { get; set; }
        public DateTimeOffset ExpiresAtTime { get; set; }
        public long? SlidingExpirationInSeconds { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }
}

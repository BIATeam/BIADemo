// <copyright file="BiaLocalCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Store object in application instance or distributed with the IDistributedCache service.
    /// </summary>
    public class BiaLocalCache : IBiaLocalCache
    {
        private readonly IMemoryCache localCache;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BiaDistributedCache> logger;

        public BiaLocalCache(IMemoryCache memoryCache, ILogger<BiaDistributedCache> logger)
        {
            this.localCache = memoryCache;
            this.logger = logger;
        }

        public async Task Add(string key, object item, double cacheDurationInMinute)
        {
            var cacheEntry = await
                this.localCache.GetOrCreateAsync(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDurationInMinute);
                    return Task.FromResult(item);
                });
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<T> Get<T>(string key)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.localCache.TryGetValue(key, out T item);
            return item;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Remove(string key)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                this.localCache.Remove(key);
            }
            catch (Exception ex)
            {
                this.logger.LogError("BIALocalCache.Remove Not in cache", ex);
                // Not in cache
            }
        }
    }
}

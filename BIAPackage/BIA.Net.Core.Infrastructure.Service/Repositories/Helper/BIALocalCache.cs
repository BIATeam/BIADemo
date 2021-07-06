// <copyright file="BIALocalCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Threading.Tasks;
    /// <summary>
    /// Store object in application instance or distributed with the IDistributedCache service
    /// </summary>
    public class BIALocalCache : IBIALocalCache
    {
        public readonly IMemoryCache localCache;

        public BIALocalCache(IMemoryCache memoryCache)
        {
            localCache = memoryCache;
        }

        public async Task Add(string key, object item, double cacheDurationInMinute)
        {
            var cacheEntry = await
                localCache.GetOrCreateAsync(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDurationInMinute);
                    return Task.FromResult(item);
                });
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<object> Get(string key)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            localCache.TryGetValue(key, out object item);
            return item;
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Remove(string key)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                localCache.Remove(key);
            }
            catch (Exception)
            {
                // Not in cache
            }
        }
    }
}

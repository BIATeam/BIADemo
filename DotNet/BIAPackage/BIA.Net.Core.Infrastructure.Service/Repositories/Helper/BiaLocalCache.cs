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
    public class BiaLocalCache : IBiaLocalCache, IBiaDistributedCache
    {
        private readonly IMemoryCache localCache;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BiaLocalCache> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaLocalCache"/> class.
        /// </summary>
        /// <param name="memoryCache">cache in memory.</param>
        /// <param name="logger">logger.</param>
        public BiaLocalCache(IMemoryCache memoryCache, ILogger<BiaLocalCache> logger)
        {
            this.localCache = memoryCache;
            this.logger = logger;
        }

        /// <summary>
        /// Add element in cache.
        /// </summary>
        /// <typeparam name="T">type of the element.</typeparam>
        /// <param name="key">key.</param>
        /// <param name="item">element.</param>
        /// <param name="cacheDurationInMinute">duration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Add<T>(string key, T item, double cacheDurationInMinute)
        {
            await this.localCache.GetOrCreateAsync(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDurationInMinute);
                    return Task.FromResult(item);
                });
        }

        /// <summary>
        /// Get element in cache.
        /// </summary>
        /// <typeparam name="T">type of the element.</typeparam>
        /// <param name="key">key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<T> Get<T>(string key)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.localCache.TryGetValue(key, out T item);
            return item;
        }

        /// <summary>
        /// Remove element from cache.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
                // Not in cache
                this.logger.LogError(ex, "BIALocalCache.Remove Not in cache");
            }
        }
    }
}

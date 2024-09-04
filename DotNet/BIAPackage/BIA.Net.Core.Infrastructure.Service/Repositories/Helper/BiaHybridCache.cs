// <copyright file="BiaHybridCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Store object in application instance or distributed with the IDistributedCache service that you find with same key.
    /// </summary>
    public class BiaHybridCache : IBiaHybridCache
    {
        /// <summary>
        /// The local cache.
        /// </summary>
        private readonly IBiaLocalCache localCache;

        /// <summary>
        /// The distributed cache.
        /// </summary>
        private readonly IBiaDistributedCache distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaHybridCache"/> class.
        /// </summary>
        /// <param name="localCache">The local cache.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public BiaHybridCache(IBiaLocalCache localCache, IBiaDistributedCache distributedCache)
        {
            this.localCache = localCache;
            this.distributedCache = distributedCache;
            this.DefaultCacheMode = CacheMode.Local;
        }

        /// <summary>
        /// Mode for the cache.
        /// </summary>
        public enum CacheMode
        {
            /// <summary>
            /// For local cache in memory.
            /// </summary>
            Local,

            /// <summary>
            /// for distributed cache in database.
            /// </summary>
            Distributed,
        }

        /// <summary>
        /// The default cache mode.
        /// </summary>
        public CacheMode DefaultCacheMode { get; set; }

        /// <summary>
        /// Add a item to cache localy or distributed => chose by default cache mode.
        /// </summary>
        /// <param name="key">The key for item.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">Validity durration of the cache.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation.</returns>
        public async Task AddDefaultMode(string key, object item, double cacheDurationInMinute)
        {
            if (this.DefaultCacheMode == CacheMode.Local)
            {
                await this.AddLocal(key, item, cacheDurationInMinute);
            }
            else
            {
                await this.AddDistributed(key, item, cacheDurationInMinute);
            }
        }

        /// <summary>
        /// Add a item to cache distributed.
        /// </summary>
        /// <param name="key">The key for item.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">Validity durration of the cache.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation.</returns>
        public async Task AddDistributed(string key, object item, double cacheDurationInMinute)
        {
            await this.distributedCache.Add(key, item, cacheDurationInMinute);
        }

        /// <summary>
        /// Add a item to cache localy.
        /// </summary>
        /// <param name="key">The key for item.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">Validity durration of the cache.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation.</returns>
        public async Task AddLocal(string key, object item, double cacheDurationInMinute)
        {
            await this.localCache.Add(key, item, cacheDurationInMinute);
        }

        /// <inheritdoc/>
        public async Task<T> GetAllSources<T>(string key)
        {
            T item = await this.GetLocal<T>(key);
            if (!object.Equals(item, default(T)))
            {
                return item;
            }

            return await this.GetDistibuted<T>(key);
        }

        /// <inheritdoc/>
        public async Task<T> GetDefaultMode<T>(string key)
        {
            if (this.DefaultCacheMode == CacheMode.Local)
            {
                return await this.GetLocal<T>(key);
            }
            else
            {
                return await this.GetDistibuted<T>(key);
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetDistibuted<T>(string key)
        {
            return await this.distributedCache.Get<T>(key);
        }

        /// <inheritdoc/>
        public async Task<T> GetLocal<T>(string key)
        {
            return await this.localCache.Get<T>(key);
        }

        /// <inheritdoc/>
        public async Task RemoveAllSources(string key)
        {
            await this.RemoveLocal(key);
            await this.RemoveDistributed(key);
        }

        /// <inheritdoc/>
        public async Task RemoveDefaultMode(string key)
        {
            if (this.DefaultCacheMode == CacheMode.Local)
            {
                await this.RemoveLocal(key);
            }
            else
            {
                await this.RemoveDistributed(key);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveDistributed(string key)
        {
            await this.distributedCache.Remove(key);
        }

        /// <inheritdoc/>
        public async Task RemoveLocal(string key)
        {
            await this.localCache.Remove(key);
        }
    }
}

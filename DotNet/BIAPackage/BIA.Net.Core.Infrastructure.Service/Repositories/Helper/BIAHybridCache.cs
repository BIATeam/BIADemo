// <copyright file="BIAHybridCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>


namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    /// <summary>
    /// Store object in application instance or distributed with the IDistributedCache service that you find with same key
    /// </summary>
    public class BIAHybridCache : IBIAHybridCache
    {
        public enum CacheMode
        {
            Local,
            Distributed
        }
        public CacheMode DefaultCacheMode { get; set; }

        IBIALocalCache localCache;
        IBIADistributedCache distributedCache;

        public BIAHybridCache(IBIALocalCache localCache, IBIADistributedCache distributedCache)
        {
            this.localCache = localCache;
            this.distributedCache = distributedCache;
            DefaultCacheMode = CacheMode.Local;
        }

        public async Task AddDefaultMode(string key, object item, double cacheDurationInMinute)
        {
            if (DefaultCacheMode == CacheMode.Local) await AddLocal(key, item, cacheDurationInMinute);
            else await AddDistributed(key, item, cacheDurationInMinute);
        }

        public async Task AddDistributed(string key, object item, double cacheDurationInMinute)
        {
            await this.distributedCache.Add(key, item, cacheDurationInMinute);
        }

        public async Task AddLocal(string key, object item, double cacheDurationInMinute)
        {
            await this.localCache.Add(key, item, cacheDurationInMinute);
        }

        public async Task<T> GetAllSources<T>(string key)
        {
            T item = await GetLocal<T>(key);
            if (item != null) return item;
            return await GetDistibuted<T>(key);
        }

        public async Task<T> GetDefaultMode<T>(string key)
        {
            if (DefaultCacheMode == CacheMode.Local) return await GetLocal<T>(key);
            else return await GetDistibuted<T>(key);
        }

        public async Task<T> GetDistibuted<T>(string key)
        {
            return await this.distributedCache.Get<T>(key);
        }

        public async Task<T> GetLocal<T>(string key)
        {
            return await this.localCache.Get<T>(key);
        }

        public async Task RemoveAllSources(string key)
        {
            await RemoveLocal(key);
            await RemoveDistributed(key);
        }

        public async Task RemoveDefaultMode(string key)
        {
            if (DefaultCacheMode == CacheMode.Local) await RemoveLocal(key);
            else await RemoveDistributed(key);
        }

        public async Task RemoveDistributed(string key)
        {
            await this.distributedCache.Remove(key);
        }

        public async Task RemoveLocal(string key)
        {
            await this.localCache.Remove(key);
        }
    }
}

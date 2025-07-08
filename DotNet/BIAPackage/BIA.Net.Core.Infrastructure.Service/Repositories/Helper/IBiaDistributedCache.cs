// <copyright file="IBiaDistributedCache.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for BiaDistributedCache.
    /// </summary>
    public interface IBiaDistributedCache
    {
        /// <summary>
        /// Add element in cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">the key.</param>
        /// <param name="item">the item.</param>
        /// <param name="cacheDurationInMinute">the cache durration.</param>
        /// <returns>the task.</returns>
        Task Add<T>(string key, T item, double cacheDurationInMinute);

        /// <summary>
        /// Get element from cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">the key.</param>
        /// <returns>the task.</returns>
        Task<T> Get<T>(string key);

        /// <summary>
        /// Remove element from the cache.
        /// </summary>
        /// <param name="key">the key.</param>
        /// <returns>the task.</returns>
        Task Remove(string key);
    }
}

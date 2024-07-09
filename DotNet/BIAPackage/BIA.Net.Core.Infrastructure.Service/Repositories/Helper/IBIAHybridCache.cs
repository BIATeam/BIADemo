// <copyright file="IBiaHybridCache.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System.Threading.Tasks;

    /// <summary>
    /// IBiaHybridCache.
    /// </summary>
    public interface IBiaHybridCache
    {
        /// <summary>
        /// Gets or sets the default cache mode.
        /// </summary>
        /// <value>
        /// The default cache mode.
        /// </value>
        BiaHybridCache.CacheMode DefaultCacheMode { get; set; }

        /// <summary>
        /// Adds the default mode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">The cache duration in minute.</param>
        /// <returns>Task.</returns>
        Task AddDefaultMode(string key, object item, double cacheDurationInMinute);

        /// <summary>
        /// Adds the distributed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">The cache duration in minute.</param>
        /// <returns>Task.</returns>
        Task AddDistributed(string key, object item, double cacheDurationInMinute);

        /// <summary>
        /// Adds the local.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">The cache duration in minute.</param>
        /// <returns>Task.</returns>
        Task AddLocal(string key, object item, double cacheDurationInMinute);

        /// <summary>
        /// Gets all sources.
        /// </summary>
        /// <typeparam name="T">type of element.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task<T> GetAllSources<T>(string key);

        /// <summary>
        /// Gets the default mode.
        /// </summary>
        /// <typeparam name="T">type of element.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task<T> GetDefaultMode<T>(string key);

        /// <summary>
        /// Gets the local.
        /// </summary>
        /// <typeparam name="T">type of element.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task<T> GetLocal<T>(string key);

        /// <summary>
        /// Gets the distibuted.
        /// </summary>
        /// <typeparam name="T">type of element.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task<T> GetDistibuted<T>(string key);

        /// <summary>
        /// Removes all sources.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task RemoveAllSources(string key);

        /// <summary>
        /// Removes the default mode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task RemoveDefaultMode(string key);

        /// <summary>
        /// Removes the distributed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task RemoveDistributed(string key);

        /// <summary>
        /// Removes the local.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task RemoveLocal(string key);
    }
}
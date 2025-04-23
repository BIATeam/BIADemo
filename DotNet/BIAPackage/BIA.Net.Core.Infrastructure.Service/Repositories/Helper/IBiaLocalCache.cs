// <copyright file="IBiaLocalCache.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System.Threading.Tasks;

    /// <summary>
    /// IBiaLocalCache.
    /// </summary>
    public interface IBiaLocalCache
    {
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="cacheDurationInMinute">The cache duration in minute.</param>
        /// <returns>Task.</returns>
        Task Add<T>(string key, T item, double cacheDurationInMinute);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task<T> Get<T>(string key);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task.</returns>
        Task Remove(string key);
    }
}

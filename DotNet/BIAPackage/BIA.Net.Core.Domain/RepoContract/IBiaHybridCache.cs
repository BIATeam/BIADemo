// <copyright file="IBiaHybridCache.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods to manage hybrid cache entries.
    /// </summary>
    public interface IBiaHybridCache
    {
        /// <summary>
        /// Gets a cached value or creates it using the provided factory.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="factoryExpression">The factory expression used when the cache entry is missing.</param>
        /// <param name="teamId">The team identifier used to scope the cache key.</param>
        /// <param name="expiration">The absolute expiration for the cache entry.</param>
        /// <param name="localCacheExpiration">The local cache expiration for the cache entry.</param>
        /// <param name="tags">The tags associated with the cache entry.</param>
        /// <param name="key">The explicit cache key to use when provided.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <returns>The cached value.</returns>
        Task<T> GetOrCreateAsync<T>(
            Expression<Func<CancellationToken, ValueTask<T>>> factoryExpression,
            int teamId = 0,
            TimeSpan? expiration = null,
            TimeSpan? localCacheExpiration = null,
            List<string> tags = null,
            string key = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes the value associated with the key if it exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all cache entries.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes cache entries for the specified team.
        /// </summary>
        /// <param name="teamId">The team identifier used to scope the cache key.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveByTeamIdAsync(int teamId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes cache entries for the specified tag.
        /// </summary>
        /// <param name="tag">The tag to remove.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveByTagAsync(string tag, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes cache entries for the specified tags.
        /// </summary>
        /// <param name="tags">The tags to remove.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveByTagAsync(List<string> tags, CancellationToken cancellationToken = default);
    }
}

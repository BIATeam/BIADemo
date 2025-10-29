// <copyright file="IUnitOfWork.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for IUnitOfWork base on the pattern 'Unit of Work'.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Attach the item to the current context.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        void Attach<TEntity>(TEntity item)
            where TEntity : class;

        /// <summary>
        /// Commit changes on the current data context.
        /// </summary>
        /// <returns>The number of element affected.</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Rollback changes in the current context.
        /// </summary>
        void RollbackChanges();

        /// <summary>
        /// Reset tracking.
        /// </summary>
        void Reset();

        /// <summary>
        /// Bulk method to add a list of item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class;

        /// <summary>
        /// Bulk method to update a list of item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class;

        /// <summary>
        /// Bulk method to remove a list of item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class;

        /// <summary>
        /// Determines whether bulk add operations are supported by the current database provider.
        /// </summary>
        /// <returns><c>true</c> if bulk add operations are supported; otherwise, <c>false</c>.</returns>
        bool IsAddBulkSupported();

        /// <summary>
        /// Determines whether bulk update operations are supported by the current database provider.
        /// </summary>
        /// <returns><c>true</c> if bulk update operations are supported; otherwise, <c>false</c>.</returns>
        bool IsUpdateBulkSupported();

        /// <summary>
        /// Determines whether bulk remove operations are supported by the current database provider.
        /// </summary>
        /// <returns><c>true</c> if bulk remove operations are supported; otherwise, <c>false</c>.</returns>
        bool IsRemoveBulkSupported();
    }
}
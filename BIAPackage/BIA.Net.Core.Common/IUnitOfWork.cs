// <copyright file="IUnitOfWork.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
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
        /// Bulk method to add a list of item.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddBulk<TEntity>(IEnumerable<TEntity> items) where TEntity : class;

        /// <summary>
        /// Bulk method to update a list of item.
        /// </summary>
        /// <param name="items">The items.</param>
        void UpdateBulk<TEntity>(IEnumerable<TEntity> items) where TEntity : class;

        /// <summary>
        /// Bulk method to remove a list of item.
        /// </summary>
        /// <param name="items">The items.</param>
        void RemoveBulk<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
    }
}
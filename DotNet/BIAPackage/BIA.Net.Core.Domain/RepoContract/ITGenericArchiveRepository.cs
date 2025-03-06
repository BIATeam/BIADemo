// <copyright file="ITGenericArchiveRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;

    /// <summary>
    /// Interface for generic archive repository of an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public interface ITGenericArchiveRepository<TEntity, TKey>
        where TEntity : class, IEntityArchivable<TKey>
    {
        /// <summary>
        /// Return the items to archive according to the filter rule.
        /// </summary>
        /// <param name="rule">Filter rule.</param>
        /// <returns><see cref="Task{IReadOnlyList{TEntity}}"/>.</returns>
        Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> rule);

        /// <summary>
        /// Update archive state of an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task SetAsArchivedAsync(TEntity entity);
    }
}
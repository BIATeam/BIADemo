// <copyright file="ITGenericCleanRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for generic clean repositories of an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public interface ITGenericCleanRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Remove all entities according to the rule.
        /// </summary>
        /// <param name="rule">Filter rule.</param>
        /// <returns><see cref="int"/> that contains the count of cleaned entities.</returns>
        Task<int> RemoveAll(Expression<Func<TEntity, bool>> rule);
    }
}

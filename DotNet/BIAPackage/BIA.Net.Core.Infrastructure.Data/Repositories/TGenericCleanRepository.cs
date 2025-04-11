// <copyright file="TGenericCleanRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Generic implementation of clean repository for an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public class TGenericCleanRepository<TEntity, TKey> : ITGenericCleanRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericCleanRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TGenericCleanRepository(IQueryableUnitOfWork context)
        {
            this.Context = context;
        }

        /// <summary>
        /// The context.
        /// </summary>
        protected IQueryableUnitOfWork Context { get; }

        /// <inheritdoc/>
        public virtual async Task<int> RemoveAll(Expression<Func<TEntity, bool>> rule)
        {
            var set = this.Context.RetrieveSet<TEntity>();
            var setWithIncludes = this.SetIncludes(set);

            var itemsToClean = await setWithIncludes.Where(rule).ToListAsync();

            set.RemoveRange(itemsToClean);
            await this.Context.CommitAsync();

            return itemsToClean.Count;
        }

        /// <summary>
        /// Set the includes to the query.
        /// </summary>
        /// <param name="query">Initial query.</param>
        /// <returns><see cref="IQueryable{TEntity}"/>.</returns>
        protected virtual IQueryable<TEntity> SetIncludes(IQueryable<TEntity> query)
        {
            return query;
        }
    }
}

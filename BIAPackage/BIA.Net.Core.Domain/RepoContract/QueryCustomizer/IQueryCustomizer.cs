// <copyright file="IQueryCustomizer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

using System.Linq;

namespace BIA.Net.Core.Domain.RepoContract.QueryCustomizer
{
    public interface IQueryCustomizer<TEntity>
    {
        /// <summary>
        /// Add custom close where or other before the Where (use for count).
        /// </summary>
        /// <param name="objectSet">Query to add includes.</param>
        /// <param name="mode">context mode to use the query.</param>
        /// <returns>>Query with the includes.</returns>
        public IQueryable<TEntity> CustomizeBefore(IQueryable<TEntity> objectSet, string queryMode);

        /// <summary>
        /// Add the include or other in the query after the automatique Where (not use for count).
        /// </summary>
        /// <param name="objectSet">Query to add includes.</param>
        /// <param name="mode">context mode to use the query.</param>
        /// <returns>>Query with the includes.</returns>
        public IQueryable<TEntity> CustomizeAfter(IQueryable<TEntity> objectSet, string queryMode);
    }
}

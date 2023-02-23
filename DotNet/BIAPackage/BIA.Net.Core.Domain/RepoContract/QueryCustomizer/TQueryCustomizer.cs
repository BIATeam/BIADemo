// <copyright file="IQueryCustomizer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

using System.Linq;

namespace BIA.Net.Core.Domain.RepoContract.QueryCustomizer
{
    public class TQueryCustomizer<TEntity> : IQueryCustomizer<TEntity>
    {
        /// <summary>
        /// Add custom close where or other before the Where (use for count).
        /// </summary>
        /// <param name="objectSet">Query to add includes.</param>
        /// <param name="mode">context mode to use the query.</param>
        /// <returns>>Query with the includes.</returns>
        public virtual IQueryable<TEntity> CustomizeBefore(IQueryable<TEntity> objectSet, string queryMode)
        {
            return objectSet;
        }

        /// <summary>
        /// Add the include for the query.
        /// </summary>
        /// <param name="objectSet">Query to add includes.</param>
        /// <param name="queryMode">context mode to use the query.</param>
        /// <returns>>Query with the includes.</returns>
        public virtual IQueryable<TEntity> CustomizeAfter(IQueryable<TEntity> objectSet, string queryMode)
        {
            return objectSet;
        }
    }
}

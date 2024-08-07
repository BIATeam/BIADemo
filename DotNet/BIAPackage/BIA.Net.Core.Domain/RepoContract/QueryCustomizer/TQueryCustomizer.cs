﻿// <copyright file="TQueryCustomizer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract.QueryCustomizer
{
    using System.Linq;

    /// <summary>
    /// TQueryCustomizer.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="BIA.Net.Core.Domain.RepoContract.QueryCustomizer.IQueryCustomizer&lt;TEntity&gt;" />
    public class TQueryCustomizer<TEntity> : IQueryCustomizer<TEntity>
    {
        /// <summary>
        /// Add custom close where or other before the Where (use for count).
        /// </summary>
        /// <param name="objectSet">Query to add includes.</param>
        /// <param name="queryMode">context mode to use the query.</param>
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

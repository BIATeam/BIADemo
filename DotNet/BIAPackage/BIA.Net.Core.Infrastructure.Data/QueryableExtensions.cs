// <copyright file="QueryableExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data
{
    using System.Linq;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.QueryOrder;

    /// <summary>
    /// Extension class for Query interface object.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies the query order.
        /// </summary>
        /// <typeparam name="TEntity">Entity Type.</typeparam>
        /// <typeparam name="TKey">Entity key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="order">The order.</param>
        /// <returns>Query where order added.</returns>
        public static IQueryable<TEntity> ApplyQueryOrder<TEntity, TKey>(this IQueryable<TEntity> source, QueryOrder<TEntity> order)
            where TEntity : class, IEntity<TKey>
        {
            foreach (var orderItem in order.GetOrderByList)
            {
                if (orderItem.Ascending)
                {
                    source = Queryable.OrderBy(source, (dynamic)orderItem.Expression);
                }
                else
                {
                    source = Queryable.OrderByDescending(source, (dynamic)orderItem.Expression);
                }
            }

            foreach (var orderItem in order.GetThenByList)
            {
                if (orderItem.Ascending)
                {
                    source = Queryable.ThenBy((IOrderedQueryable<TEntity>)source, (dynamic)orderItem.Expression);
                }
                else
                {
                    source = Queryable.ThenByDescending((IOrderedQueryable<TEntity>)source, (dynamic)orderItem.Expression);
                }
            }

            return source;
        }
    }
}
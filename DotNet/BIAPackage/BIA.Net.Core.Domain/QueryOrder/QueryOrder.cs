// <copyright file="QueryOrder.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.QueryOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// Query Order.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class QueryOrder<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The order by descending list.
        /// </summary>
        private readonly IList<LambdaExpression> orderByDescendingList;

        /// <summary>
        /// The order by list.
        /// </summary>
        private readonly IList<LambdaExpression> orderByList;

        /// <summary>
        /// The then by descending list.
        /// </summary>
        private readonly IList<LambdaExpression> thenByDescendingList;

        /// <summary>
        /// The then by list.
        /// </summary>
        private readonly IList<LambdaExpression> thenByList;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryOrder{TEntity}"/> class.
        /// </summary>
        public QueryOrder()
        {
            this.orderByList = new List<LambdaExpression>();
            this.orderByDescendingList = new List<LambdaExpression>();
            this.thenByList = new List<LambdaExpression>();
            this.thenByDescendingList = new List<LambdaExpression>();
        }

        /// <summary>
        /// Gets the order by descending list.
        /// </summary>
        /// <returns>List of order.</returns>
        public IList<LambdaExpression> GetOrderByDescendingList => this.orderByDescendingList;

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <returns>List of order.</returns>
        public IList<LambdaExpression> GetOrderByList => this.orderByList;

        /// <summary>
        /// Gets the then by descending list.
        /// </summary>
        /// <returns>List of order.</returns>
        public IList<LambdaExpression> GetThenByDescendingList => this.thenByDescendingList;

        /// <summary>
        /// Gets the then by list.
        /// </summary>
        /// <returns>List of order.</returns>
        public IList<LambdaExpression> GetThenByList => this.thenByList;

        /// <summary>
        /// Set the order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderExpression)
        {
            // Aggregate
            this.orderByList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderBy(LambdaExpression orderExpression)
        {
            // Aggregate
            this.orderByList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the descending order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderExpression)
        {
            // Aggregate
            this.orderByDescendingList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the descending order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderByDescending(LambdaExpression orderExpression)
        {
            // Aggregate
            this.orderByDescendingList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the secondary order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> orderExpression)
        {
            // Aggregate
            this.thenByList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the secondary order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenBy(LambdaExpression orderExpression)
        {
            // Aggregate
            this.thenByList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the secondary descending order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> orderExpression)
        {
            // Aggregate
            this.thenByDescendingList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Set the secondary descending order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenByDescending(LambdaExpression orderExpression)
        {
            // Aggregate
            this.thenByDescendingList.Add(orderExpression);

            return this;
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <returns>List of order.</returns>
        public string WriteInfo()
        {
            var info = new StringBuilder();

            info.Append("OrderBy : ");
            info.Append(string.Join(",", GetMember(this.orderByList)));

            info.Append(" - OrderByDescending : ");
            info.Append(string.Join(",", GetMember(this.orderByDescendingList)));

            info.Append(" - ThenBy : ");
            info.Append(string.Join(",", GetMember(this.thenByList)));

            info.Append(" - ThenByDescending : ");
            info.Append(string.Join(",", GetMember(this.thenByDescendingList)));

            return info.ToString();
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>List of member name.</returns>
        private static IEnumerable<string> GetMember(IEnumerable<LambdaExpression> expressions)
        {
            return expressions.Select(item => item.Body).OfType<MemberExpression>().Select(memberEx => memberEx.ToString()).ToList();
        }
    }
}
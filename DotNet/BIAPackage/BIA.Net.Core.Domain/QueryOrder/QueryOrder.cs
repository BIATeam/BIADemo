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
        /// Initializes a new instance of the <see cref="QueryOrder{TEntity}"/> class.
        /// </summary>
        public QueryOrder()
        {
            this.GetOrderByList = new List<ItemOrder>();
            this.GetThenByList = new List<ItemOrder>();
        }

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <returns>List of order.</returns>
        public IList<ItemOrder> GetOrderByList { get; }

        /// <summary>
        /// Gets the then by list.
        /// </summary>
        /// <returns>List of order.</returns>
        public IList<ItemOrder> GetThenByList { get; }


        /// <summary>
        /// Set the order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderExpression">The order expression.</param>
        /// <param name="ascending">The direction of the sort.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderExpression, bool ascending = true)
        {
            if (this.GetOrderByList.Count == 0)
            {
                this.GetOrderByList.Add(new ItemOrder { Expression = orderExpression, Ascending = ascending });
            }
            else
            {
                this.GetThenByList.Add(new ItemOrder { Expression = orderExpression, Ascending = ascending });
            }

            return this;
        }

        /// <summary>
        /// Set the order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <param name="ascending">The direction of the sort.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderBy(LambdaExpression orderExpression, bool ascending = true)
        {
            if (this.GetOrderByList.Count == 0)
            {
                this.GetOrderByList.Add(new ItemOrder { Expression = orderExpression, Ascending = ascending });
            }
            else
            {
                this.GetThenByList.Add(new ItemOrder { Expression = orderExpression, Ascending = ascending });
            }

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
            return this.OrderBy(orderExpression, false);
        }

        /// <summary>
        /// Set the descending order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> OrderByDescending(LambdaExpression orderExpression)
        {
            return this.OrderBy(orderExpression, false);
        }

        /// <summary>
        /// Set the secondary order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderExpression">The order expression.</param>
        /// <param name="ascending">The direction of the sort.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> orderExpression, bool ascending = true)
        {
            this.GetThenByList.Add(new ItemOrder { Expression = orderExpression, Ascending = ascending });
            return this;
        }

        /// <summary>
        /// Set the secondary order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <param name="ascending">The direction of the sort.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenBy(LambdaExpression orderExpression, bool ascending = true)
        {
            this.GetThenByList.Add(new ItemOrder { Expression = orderExpression, Ascending = ascending });
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
            return this.ThenBy(orderExpression, false);
        }

        /// <summary>
        /// Set the secondary descending order.
        /// </summary>
        /// <param name="orderExpression">The order expression.</param>
        /// <returns>Current class object.</returns>
        public QueryOrder<TEntity> ThenByDescending(LambdaExpression orderExpression)
        {
            return this.ThenBy(orderExpression, false);
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <returns>List of order.</returns>
        public string WriteInfo()
        {
            var info = new StringBuilder();

            info.Append("OrderBy : ");

            foreach (var item in this.GetOrderByList)
            {
                info.Append("," + ((MemberExpression)item.Expression.Body)?.Member.Name + ((!item.Ascending) ? " Desc" : string.Empty));
            }
            foreach (var item in this.GetThenByList)
            {
                info.Append("," + ((MemberExpression)item.Expression.Body)?.Member.Name + ((!item.Ascending) ? " Desc" : string.Empty));
            }

            return info.ToString();
        }

        /// <summary>
        /// Item Order.
        /// </summary>
        public class ItemOrder
        {
            /// <summary>
            /// Lambda Expression.
            /// </summary>
            public LambdaExpression Expression { get; set; }

            /// <summary>
            /// Direction of the sort.
            /// </summary>
            public bool Ascending { get; set; }
        }
    }
}
// <copyright file="QueryOrderExtensions.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.QueryOrder
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Query Order Extensions.
    /// </summary>
    public static class QueryOrderExtensions
    {
        /// <summary>
        /// Gets the by expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryOrder">The query order.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="ascending">If set to <c>true</c> [ascending].</param>
        public static void GetByExpression<TEntity>(this QueryOrder<TEntity> queryOrder, LambdaExpression expression, bool ascending)
            where TEntity : class
        {
            LambdaExpression orderExpression = expression;
            if (SpecificationHelper.IsCollectionType(expression.ReturnType))
            {
                var orderExpressionCall = Expression.Call(typeof(Enumerable), "FirstOrDefault", new[] { typeof(string) }, expression.Body);
                ParameterExpression parameterExpression = expression.Parameters.FirstOrDefault();
                orderExpression = Expression.Lambda<Func<TEntity, string>>(orderExpressionCall, parameterExpression);
            }



            if (queryOrder.GetOrderByDescendingList.Count > 0 && queryOrder.GetOrderByList.Count > 0)
            {
                if (ascending)
                {
                    queryOrder.ThenBy(orderExpression);
                }
                else
                {
                    queryOrder.ThenByDescending(orderExpression);
                }
            }
            else
            {
                if (ascending)
                {
                    queryOrder.OrderBy(orderExpression);
                }
                else
                {
                    queryOrder.OrderByDescending(orderExpression);
                }
            }
        }
    }
}
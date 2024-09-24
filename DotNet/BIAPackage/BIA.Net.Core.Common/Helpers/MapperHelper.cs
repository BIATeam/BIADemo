// <copyright file="MapperHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides methods and extensions helpers for mappers.
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// Produces the combination of two mapping expressions.
        /// </summary>
        /// <typeparam name="TParam">Type of mapping expressions parameter.</typeparam>
        /// <typeparam name="TResult">Type of mapping expressions result.</typeparam>
        /// <param name="firstMapping">First mapping expression to combine.</param>
        /// <param name="secondMapping">Second mapping expression to combine.</param>
        /// <returns>Combination <see cref="Expression"/> of the two mapping expressions.</returns>
        /// <exception cref="InvalidOperationException">One of the expression to combine is not correctly formated.</exception>
        public static Expression<Func<TParam, TResult>> CombineMapping<TParam, TResult>(
            this Expression<Func<TParam, TResult>> firstMapping,
            Expression<Func<TParam, TResult>> secondMapping)
        {
            var firstMappingParameterExpression = firstMapping.Parameters[0];
            var secondMappingParameterExpression = secondMapping.Parameters[0];

            if (!firstMappingParameterExpression.Name.Equals(secondMappingParameterExpression.Name))
            {
                throw new InvalidOperationException("Parameter name of mapping expressions must be similar.");
            }

            if (firstMapping.Body is not MemberInitExpression firstMappingBodyExpression)
            {
                throw new InvalidOperationException($"First mapping expression must be a {nameof(MemberInitExpression)}.");
            }

            if (secondMapping.Body is not MemberInitExpression secondMappingBodyExpression)
            {
                throw new InvalidOperationException($"Second mapping expression must be a {nameof(MemberInitExpression)}.");
            }

            var expressionParameterReplacer = new ExpressionParameterReplacer(secondMappingParameterExpression, firstMappingParameterExpression);
            var updatedSecondMappingBody = (MemberInitExpression)expressionParameterReplacer.Visit(secondMappingBodyExpression);

            var combinedBindings = new List<MemberBinding>(firstMappingBodyExpression.Bindings);
            combinedBindings.AddRange(updatedSecondMappingBody.Bindings);

            var combinedBody = Expression.MemberInit(Expression.New(typeof(TResult)), combinedBindings);
            return Expression.Lambda<Func<TParam, TResult>>(combinedBody, firstMappingParameterExpression);
        }

        /// <summary>
        /// Extension of <see cref="ExpressionVisitor"/> to replace inside an <see cref="Expression"/>'s tree the <paramref name="oldParameterExpression"/> by <paramref name="newParameterExpression"/>.
        /// </summary>
        private sealed class ExpressionParameterReplacer(ParameterExpression oldParameterExpression, ParameterExpression newParameterExpression)
            : ExpressionVisitor
        {
            /// <inheritdoc cref="ExpressionVisitor.VisitParameter(ParameterExpression)"/>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == oldParameterExpression ? newParameterExpression : base.VisitParameter(node);
            }
        }
    }
}

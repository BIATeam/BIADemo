// <copyright file="ExpressionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides extensions for expressions.
    /// </summary>
    public static class ExpressionExtensions
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
            if (firstMapping.Body is not MemberInitExpression firstMappingBodyExpression)
            {
                throw new InvalidOperationException($"First mapping expression must be a {nameof(MemberInitExpression)}.");
            }

            if (secondMapping.Body is not MemberInitExpression secondMappingBodyExpression)
            {
                throw new InvalidOperationException($"Second mapping expression must be a {nameof(MemberInitExpression)}.");
            }

            var firstMappingParameterExpression = firstMapping.Parameters[0];
            var secondMappingParameterExpression = secondMapping.Parameters[0];

            var expressionParameterReplacer = new ExpressionParameterReplacer(secondMappingParameterExpression, firstMappingParameterExpression);
            var updatedSecondMappingBody = (MemberInitExpression)expressionParameterReplacer.Visit(secondMappingBodyExpression);

            var combinedBindings = new List<MemberBinding>(firstMappingBodyExpression.Bindings);
            combinedBindings.AddRange(updatedSecondMappingBody.Bindings);

            var combinedBody = Expression.MemberInit(Expression.New(typeof(TResult)), combinedBindings);
            return Expression.Lambda<Func<TParam, TResult>>(combinedBody, firstMappingParameterExpression);
        }

        /// <summary>
        /// Extend an expression selector with another one.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="firstSelector">First selector expression.</param>
        /// <param name="secondSelector">Second selector expression.</param>
        /// <returns>Extended <see cref="Expression"/>.</returns>
        public static Expression<Func<T, bool>> CombineSelector<T>(this Expression<Func<T, bool>> firstSelector, Expression<Func<T, bool>> secondSelector)
        {
            if (firstSelector == null)
            {
                return secondSelector;
            }

            if (secondSelector == null)
            {
                return firstSelector;
            }

            var firstSelectorParameter = firstSelector.Parameters[0];
            var expressionParameterReplacer = new ExpressionParameterReplacer(secondSelector.Parameters[0], firstSelectorParameter);
            var updatedSecondSelectorBody = expressionParameterReplacer.Visit(secondSelector.Body);

            var combinedBody = Expression.AndAlso(firstSelector.Body, updatedSecondSelectorBody);
            return Expression.Lambda<Func<T, bool>>(combinedBody, firstSelectorParameter);
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

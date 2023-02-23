// <copyright file="ParameterRebinder.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Helper for binder parameters without use Invoke method in expressions (this methods is not
    /// supported in all LINQ query providers, for example in LINQ2Entities is not supported).
    /// </summary>
    public sealed class ParameterRebinder : ExpressionVisitor
    {
        /// <summary>
        /// The map.
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">Map specification.</param>
        private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replace parameters in expression with a Map information.
        /// </summary>
        /// <param name="map">Map information.</param>
        /// <param name="expression">Expression to replace parameters.</param>
        /// <returns>Expression with parameters replaced.</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression expression)
        {
            return new ParameterRebinder(map).Visit(expression);
        }

        /// <summary>
        /// Visit pattern method.
        /// </summary>
        /// <param name="node">A Parameter expression.</param>
        /// <returns>New visited expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (this.map.TryGetValue(node, out var replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
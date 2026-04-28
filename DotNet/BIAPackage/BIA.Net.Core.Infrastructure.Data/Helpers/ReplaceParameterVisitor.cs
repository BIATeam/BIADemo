// <copyright file="ReplaceParameterVisitor.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System.Linq.Expressions;

    /// <summary>
    /// Visits expression trees and replaces a specific parameter expression with a new expression.
    /// </summary>
    /// <remarks>
    /// This visitor is useful when composing or rewriting lambda expressions by substituting
    /// one parameter reference with a different expression instance.
    /// </remarks>
    public class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression oldParam;
        private readonly Expression newExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceParameterVisitor"/> class.
        /// </summary>
        /// <param name="oldParam">The parameter expression to replace.</param>
        /// <param name="newExpr">The replacement expression that should substitute the old parameter.</param>
        public ReplaceParameterVisitor(ParameterExpression oldParam, Expression newExpr)
        {
            this.oldParam = oldParam;
            this.newExpr = newExpr;
        }

        /// <summary>
        /// Visits a <see cref="ParameterExpression"/> node and replaces it if it matches the configured old parameter.
        /// </summary>
        /// <param name="node">The parameter expression node currently being visited.</param>
        /// <returns>
        /// The replacement expression when the node matches <see cref="oldParam"/>, otherwise the original visited node.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParam ? newExpr : base.VisitParameter(node);
        }
    }
}

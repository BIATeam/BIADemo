// <copyright file="TrueSpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// True specification
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public sealed class TrueSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Check if the specification is satisfied by the condition.
        /// </summary>
        /// <returns>Specification {TEntity}.</returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            // Create "result variable" transform adhoc execution plan in prepared plan
            var result = true;

            Expression<Func<TEntity, bool>> trueExpression = t => result;
            return trueExpression;
        }
    }
}
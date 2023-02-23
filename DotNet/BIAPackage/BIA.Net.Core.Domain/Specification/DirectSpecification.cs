// <copyright file="DirectSpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// A Direct Specification is a simple implementation of specification that acquire this from a
    /// lambda expression in constructor.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification.</typeparam>
    public sealed class DirectSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The matching criteria.
        /// </summary>
        private readonly Expression<Func<TEntity, bool>> matchingCriteria;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSpecification{TEntity}"/> class.
        /// </summary>
        /// <param name="matchingCriteria">The matching criteria.</param>
        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            this.matchingCriteria = matchingCriteria ?? throw new ArgumentNullException(nameof(matchingCriteria));
        }

        /// <summary>
        /// Satisfied By Pattern.
        /// </summary>
        /// <returns>The expression.</returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return this.matchingCriteria;
        }
    }
}
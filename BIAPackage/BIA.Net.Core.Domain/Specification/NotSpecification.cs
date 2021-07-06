// <copyright file="NotSpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Not Specification convert a original specification with NOT logic operator.
    /// </summary>
    /// <typeparam name="TEntity">Type of element for this specification.</typeparam>
    public sealed class NotSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The original criteria.
        /// </summary>
        private readonly Expression<Func<TEntity, bool>> originalCriteria;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification{TEntity}"/> class.
        /// </summary>
        /// <param name="originalSpecification">The original specification.</param>
        public NotSpecification(ISpecification<TEntity> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException(nameof(originalSpecification));
            }

            this.originalCriteria = originalSpecification.SatisfiedBy();
        }

        /// <summary>
        /// Satisfied the by.
        /// </summary>
        /// <returns>Lambda expression.</returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(this.originalCriteria.Body), this.originalCriteria.Parameters.Single());
        }
    }
}
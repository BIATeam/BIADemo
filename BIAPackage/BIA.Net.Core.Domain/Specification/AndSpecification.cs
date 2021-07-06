// <copyright file="AndSpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// A logic AND Specification.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification.</typeparam>
    public sealed class AndSpecification<TEntity> : CompositeSpecification<TEntity>
       where TEntity : class
    {
        /// <summary>
        /// The left side specification.
        /// </summary>
        private readonly ISpecification<TEntity> leftSideSpecification;

        /// <summary>
        /// The right side specification.
        /// </summary>
        private readonly ISpecification<TEntity> rightSideSpecification;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification{TEntity}"/> class.
        /// </summary>
        /// <param name="leftSide">Left side specification.</param>
        /// <param name="rightSide">Right side specification.</param>
        public AndSpecification(ISpecification<TEntity> leftSide, ISpecification<TEntity> rightSide)
        {
            this.leftSideSpecification = leftSide ?? throw new ArgumentNullException(nameof(leftSide));
            this.rightSideSpecification = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
        }

        /// <summary>
        /// Gets the Left side specification.
        /// </summary>
        public override ISpecification<TEntity> LeftSideSpecification => this.leftSideSpecification;

        /// <summary>
        /// Gets the Right side specification.
        /// </summary>
        public override ISpecification<TEntity> RightSideSpecification => this.rightSideSpecification;

        /// <summary>
        /// Satisfied the by.
        /// </summary>
        /// <returns>The expression.</returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            var left = this.leftSideSpecification.SatisfiedBy();
            var right = this.rightSideSpecification.SatisfiedBy();

            return left.And(right);
        }
    }
}
// <copyright file="OrSpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// A Logic OR Specification.
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification.</typeparam>
    public sealed class OrSpecification<T> : CompositeSpecification<T>
         where T : class
    {
        /// <summary>
        /// The left side specification.
        /// </summary>
        private readonly ISpecification<T> leftSideSpecification;

        /// <summary>
        /// The right side specification.
        /// </summary>
        private readonly ISpecification<T> rightSideSpecification;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            this.leftSideSpecification = leftSide ?? throw new ArgumentNullException(nameof(leftSide));
            this.rightSideSpecification = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
        }

        /// <summary>
        /// Gets the Left side specification.
        /// </summary>
        public override ISpecification<T> LeftSideSpecification => this.leftSideSpecification;

        /// <summary>
        /// Gets the Right side specification.
        /// </summary>
        public override ISpecification<T> RightSideSpecification => this.rightSideSpecification;

        /// <summary>
        /// Satisfied the by.
        /// </summary>
        /// <returns>Lambda expression.</returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = this.leftSideSpecification.SatisfiedBy();
            var right = this.rightSideSpecification.SatisfiedBy();

            return left.Or(right);
        }
    }
}
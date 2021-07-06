// <copyright file="Specification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Represent a Specification used with the pattern of the same name.
    /// </summary>
    /// <typeparam name="TEntity">Type of item in the criteria.</typeparam>
    public abstract class Specification<TEntity> : ISpecification<TEntity>
         where TEntity : class
    {
        /// <summary>
        /// Not specification.
        /// </summary>
        /// <param name="specification">Specification to negate.</param>
        /// <returns>New specification.</returns>
        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        /// And operator.
        /// </summary>
        /// <param name="leftSideSpecification">Left operand in this AND operation.</param>
        /// <param name="rightSideSpecification">Right operand in this AND operation.</param>
        /// <returns>New specification.</returns>
        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            if (leftSideSpecification == null) return rightSideSpecification;
            if (rightSideSpecification == null) return leftSideSpecification;
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Or operator.
        /// </summary>
        /// <param name="leftSideSpecification">Left operand in this OR operation.</param>
        /// <param name="rightSideSpecification">Right operand in this OR operation.</param>
        /// <returns>New specification.</returns>
        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Override operator false, only for support AND OR operators.
        /// </summary>
        /// <param name="specification">Specification instance.</param>
        /// <returns>See False operator in C#.</returns>
        public static bool operator false(Specification<TEntity> specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            return false;
        }

        /// <summary>
        /// Override operator True, only for support AND OR operators.
        /// </summary>
        /// <param name="specification">Specification instance.</param>
        /// <returns>See True operator in C#.</returns>
        public static bool operator true(Specification<TEntity> specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            return true;
        }

        /// <summary>
        /// Satisfied by Specification pattern method.
        /// </summary>
        /// <returns>Expression that satisfy this specification.</returns>
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}
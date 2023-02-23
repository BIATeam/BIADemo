// <copyright file="CompositeSpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    /// <summary>
    /// Base class for composite specifications.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification.</typeparam>
    public abstract class CompositeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets the Left side specification for this composite element.
        /// </summary>
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Gets the Right side specification for this composite element.
        /// </summary>
        public abstract ISpecification<TEntity> RightSideSpecification { get; }
    }
}
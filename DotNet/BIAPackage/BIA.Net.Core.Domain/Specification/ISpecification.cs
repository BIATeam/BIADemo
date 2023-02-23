// <copyright file="ISpecification.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The interface used for the specification pattern.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Check if this specification is satisfied by a specific expression lambda.
        /// </summary>
        /// <returns>Lambda expression.</returns>
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}
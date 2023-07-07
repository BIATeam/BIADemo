// <copyright file="BaseEntityMapper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseEntityMapper<TEntity>
    {
        /// <summary>
        /// Gets the collection used for expressions to access fields.
        /// </summary>
        public virtual ExpressionCollection<TEntity> ExpressionCollection
        {
            get
            {
#pragma warning disable CA1065 // Ne pas lever d'exceptions dans les emplacements inattendus
#pragma warning disable S2372 // Exceptions should not be thrown from property getters
                throw new BadBiaFrameworkUsageException("This mapper is not build for list, or the implementation of ExpressionCollection is missing.");
#pragma warning restore S2372 // Exceptions should not be thrown from property getters
#pragma warning restore CA1065 // Ne pas lever d'exceptions dans les emplacements inattendus
            }
        }
    }
}
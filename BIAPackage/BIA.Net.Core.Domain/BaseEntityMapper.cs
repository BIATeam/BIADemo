// <copyright file="BaseMapper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseEntityMapper<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets the collection used for expressions to access fields.
        /// </summary>
        public virtual ExpressionCollection<TEntity> ExpressionCollection
        {
            get
            {
                throw new NotImplementedException("This mapper is not build for list, or the implementation of ExpressionCollection is missing.");
            }
        }
    }
}
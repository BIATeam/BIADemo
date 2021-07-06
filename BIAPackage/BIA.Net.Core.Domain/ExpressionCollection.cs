// <copyright file="ExpressionCollection.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using BIA.Net.Core.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Expression Collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class ExpressionCollection<TEntity> : BIADictionary<LambdaExpression>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="expression">The expression.</param>
        public void Add<TKey>(string key, Expression<Func<TEntity, TKey>> expression)
        {
            this.internalDictionary.Add(key, expression);
        }
    }
}
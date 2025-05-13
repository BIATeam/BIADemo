// <copyright file="ExpressionCollection.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;

    /// <summary>
    /// Expression Collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class ExpressionCollection<TEntity> : BiaDictionary<LambdaExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionCollection{TEntity}"/> class.
        /// </summary>
        public ExpressionCollection()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionCollection{TEntity}"/> class.
        /// </summary>
        /// <param name="initial">The initial.</param>
        public ExpressionCollection(ExpressionCollection<TEntity> initial)
            : base()
        {
            foreach (var elem in initial)
            {
                this.InternalDictionary.Add(elem.Key, elem.Value);
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="expression">The expression.</param>
        public void Add<TKey>(string key, Expression<Func<TEntity, TKey>> expression)
        {
            this.InternalDictionary.Add(key, expression);
        }
    }
}
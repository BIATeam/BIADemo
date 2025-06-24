// <copyright file="ExpressionCollection.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
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
        /// Initializes a new instance of the <see cref="ExpressionCollection{TEntity}"/> class.
        /// </summary>
        /// <param name="initial">The initial.</param>
        /// <param name="newElements">The new elements to add to the expression collection.</param>
        public ExpressionCollection(ExpressionCollection<TEntity> initial, ExpressionCollection<TEntity> newElements)
            : base()
        {
            foreach (var elem in initial)
            {
                this.InternalDictionary.Add(elem.Key, elem.Value);
            }
            foreach (var elem in newElements)
            {
                if (this.InternalDictionary.ContainsKey(elem.Key))
                {
                    this.InternalDictionary[elem.Key] = elem.Value;
                }
                else
                {
                    this.InternalDictionary.Add(elem.Key, elem.Value);
                }
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
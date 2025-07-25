﻿// <copyright file="BiaDictionary.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Expression Collection.
    /// </summary>
    /// <typeparam name="TElem">The type of the entity.</typeparam>
    public class BiaDictionary<TElem> : IEnumerable<KeyValuePair<string, TElem>>
    {
        /// <summary>
        /// The internal dictionary.
        /// </summary>
        private readonly Dictionary<string, TElem> internalDictionary
            = new (StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets the internal dictionary.
        /// </summary>
        /// <value>
        /// The internal dictionary.
        /// </value>
        public Dictionary<string, TElem> InternalDictionary => this.internalDictionary;

        /// <summary>
        /// Gets or sets the <see cref="TElem"/> with the specified key.
        /// </summary>
        /// <value>The <see cref="TElem"/>.</value>
        /// <param name="key">The key.</param>
        /// <returns>The selected <see cref="TElem"/>.</returns>
        public TElem this[string key]
        {
            get
            {
                return this.internalDictionary[key];
            }

            set
            {
                this.internalDictionary[key] = value;
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="elem">The element.</param>
        public void Add(string key, TElem elem)
        {
            this.internalDictionary.Add(key, elem);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, TElem>> GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The specified key contains key.</returns>
        public bool ContainsKey(string key)
        {
            return this.internalDictionary.ContainsKey(key);
        }
    }
}

// <copyright file="ImmutableDictionaryBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Immutable;

    /// <summary>
    /// Helper to use to initialize inline an ImmutableDictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <example>
    /// var d = new ImmutableDictionaryBuilder.<int, string>()
    /// {
    ///    { 1, "One" },
    ///    { 2, "Two" },
    ///    { 3, "Three" }
    /// }.ToImmutable();
    /// </example>
    public class ImmutableDictionaryBuilder<TKey, TValue> : IEnumerable
    {
        private readonly ImmutableDictionary<TKey, TValue>.Builder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableDictionaryBuilder{TKey, TValue}"/> class.
        /// </summary>
        public ImmutableDictionaryBuilder()
        {
            this.builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
        }

        /// <summary>
        /// set the value.
        /// </summary>
        /// <param name="key">Type of the key.</param>
        /// <returns>none.</returns>
        public TValue this[TKey key]
        {
            set { this.builder[key] = value; }
        }

        /// <summary>
        /// Ad un element.
        /// </summary>
        /// <param name="key">Type of the key.</param>
        /// <param name="value">Type of the value.</param>
        public void Add(TKey key, TValue value) => this.builder.Add(key, value);

        /// <summary>
        /// Generate the Immuable collection.
        /// </summary>
        /// <returns>the Immuable collection.</returns>
        public ImmutableDictionary<TKey, TValue> ToImmutable() => this.builder.ToImmutable();

        /// <summary>
        /// Get Enumerator().
        /// </summary>
        /// <returns>Exception.</returns>
        /// <exception cref="NotImplementedException">Error.</exception>
        public IEnumerator GetEnumerator()
        {
            // Only implementing IEnumerable because collection initializer
            // syntax is unavailable if you don't.
            throw new NotImplementedException();
        }
    }
}

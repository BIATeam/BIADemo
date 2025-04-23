// <copyright file="ImmutableListBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Immutable;

    /// <summary>
    /// Helper to use to initialize inline an ImmutableList.
    /// </summary>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <example>
    /// var d = new ImmutableListBuilder.<string>()
    /// {
    ///    { "One" },
    ///    { "Two" },
    ///    { "Three" }
    /// }.ToImmutable();
    /// </example>
    public class ImmutableListBuilder<TValue> : IEnumerable
    {
        private readonly ImmutableList<TValue>.Builder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableListBuilder{TValue}"/> class.
        /// </summary>
        public ImmutableListBuilder()
        {
            this.builder = ImmutableList.CreateBuilder<TValue>();
        }

        /// <summary>
        /// Ad un element.
        /// </summary>
        /// <param name="value">Type of the value.</param>
        public void Add(TValue value) => this.builder.Add(value);

        /// <summary>
        /// Generate the Immuable collection.
        /// </summary>
        /// <returns>the Immuable collection.</returns>
        public ImmutableList<TValue> ToImmutable() => this.builder.ToImmutable();

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

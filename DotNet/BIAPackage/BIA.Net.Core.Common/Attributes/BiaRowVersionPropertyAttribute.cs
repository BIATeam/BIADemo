// <copyright file="BiaRowVersionPropertyAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Attributes
{
    using System;
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// Specifies that a property represents a row version for a specific database provider.
    /// </summary>
    /// <param name="provider">The database provider.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BiaRowVersionPropertyAttribute(DbProvider provider) : Attribute
    {
        /// <summary>
        /// Gets the database provider.
        /// </summary>
        public DbProvider Provider { get; } = provider;
    }
}

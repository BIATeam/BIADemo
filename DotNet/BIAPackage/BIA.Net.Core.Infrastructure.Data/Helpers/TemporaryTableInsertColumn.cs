// <copyright file="TemporaryTableInsertColumn.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a column and its associated value formatter for inserting into a temporary table.
    /// </summary>
    /// <typeparam name="TValue">The type of values to insert.</typeparam>
    internal class TemporaryTableInsertColumn<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryTableInsertColumn{TValue}"/> class.
        /// </summary>
        /// <param name="columnName">The name of the column to insert values into.</param>
        /// <param name="valueSelector">Function to extract the value for this column from the source object.</param>
        /// <param name="formatValue">Optional function to format the value for SQL. If null, the value is used as-is.</param>
        public TemporaryTableInsertColumn(
            string columnName,
            Func<TValue, object> valueSelector,
            Func<object, object> formatValue = null)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            ArgumentNullException.ThrowIfNull(valueSelector);

            this.ColumnName = columnName;
            this.ValueSelector = valueSelector;
            this.FormatValue = formatValue;
        }

        /// <summary>
        /// Gets the name of the column to insert values into.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the function to extract the value for this column from the source object.
        /// </summary>
        public Func<TValue, object> ValueSelector { get; }

        /// <summary>
        /// Gets the optional function to format the value for SQL.
        /// </summary>
        public Func<object, object> FormatValue { get; }
    }
}

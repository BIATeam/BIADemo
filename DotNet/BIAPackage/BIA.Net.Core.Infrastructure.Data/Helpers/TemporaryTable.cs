// <copyright file="TemporaryTable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a definition of a temporary table with its schema and insertion configuration.
    /// Encapsulates all metadata needed to create, populate, and interact with a temporary table.
    /// </summary>
    internal class TemporaryTable
    {
        private readonly List<TemporaryTableColumnDefinition> columns;
        private readonly Dictionary<string, object> insertColumnsMap; // columnName -> TemporaryTableInsertColumn<TValue>

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryTable"/> class.
        /// </summary>
        /// <param name="name">The name of the temporary table.</param>
        public TemporaryTable(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.columns = new List<TemporaryTableColumnDefinition>();
            this.insertColumnsMap = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the name of the temporary table.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the column definitions (read-only).
        /// </summary>
        public IReadOnlyList<TemporaryTableColumnDefinition> Columns => this.columns.AsReadOnly();

        /// <summary>
        /// Gets the insert column definitions for a specific column name.
        /// </summary>
        /// <typeparam name="TValue">The type of values in this column.</typeparam>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The insert column definition for the specified column.</returns>
        /// <exception cref="InvalidOperationException">If the column is not defined.</exception>
        public TemporaryTableInsertColumn<TValue> GetInsertColumn<TValue>(string columnName)
        {
            if (!this.insertColumnsMap.TryGetValue(columnName, out var insertColumn))
            {
                throw new InvalidOperationException(
                    $"No insert column defined for '{columnName}'. " +
                    $"Call AddInsertColumn<T>('{columnName}', ...) first.");
            }

            if (insertColumn is not TemporaryTableInsertColumn<TValue> typedColumn)
            {
                throw new InvalidOperationException(
                    $"Insert column '{columnName}' is not of type '{typeof(TValue).Name}'.",
                    new InvalidCastException());
            }

            return typedColumn;
        }

        /// <summary>
        /// Adds a column definition to the temporary table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="sqlType">The SQL type of the column.</param>
        /// <param name="isPrimaryKey">Indicates whether the column is a primary key.</param>
        /// <param name="isNullable">Indicates whether the column is nullable.</param>
        /// <returns>The current instance for method chaining.</returns>
        public TemporaryTable AddColumn(
            string columnName,
            string sqlType,
            bool isPrimaryKey = false,
            bool isNullable = false)
        {
            var column = new TemporaryTableColumnDefinition(columnName, sqlType, isPrimaryKey, isNullable);
            this.columns.Add(column);
            return this;
        }

        /// <summary>
        /// Adds an insert column definition for a specific column and value type.
        /// </summary>
        /// <typeparam name="TValue">The type of values to insert into this column.</typeparam>
        /// <param name="columnName">The name of the column to insert into.</param>
        /// <param name="valueSelector">Function to extract the value from the source object.</param>
        /// <param name="formatValue">Optional function to format the extracted value.</param>
        /// <returns>The current instance for method chaining.</returns>
        public TemporaryTable AddInsertColumn<TValue>(
            string columnName,
            Func<TValue, object> valueSelector,
            Func<object, object> formatValue = null)
        {
            var insertColumn = new TemporaryTableInsertColumn<TValue>(columnName, valueSelector, formatValue);
            this.insertColumnsMap[columnName] = insertColumn;
            return this;
        }
    }
}

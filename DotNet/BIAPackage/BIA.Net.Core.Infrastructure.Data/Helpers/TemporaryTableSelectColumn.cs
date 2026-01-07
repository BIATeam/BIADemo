// <copyright file="TemporaryTableSelectColumn.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;

    /// <summary>
    /// Represents a column to select in a SELECT query with table information.
    /// </summary>
    public class TemporaryTableSelectColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryTableSelectColumn"/> class.
        /// </summary>
        /// <param name="tableAlias">The alias of the table containing the column.</param>
        /// <param name="columnName">The name of the column to select.</param>
        /// <param name="alias">Optional alias for the selected column in the result set.</param>
        public TemporaryTableSelectColumn(string tableAlias, string columnName, string alias = null)
        {
            if (string.IsNullOrWhiteSpace(tableAlias))
            {
                throw new ArgumentNullException(nameof(tableAlias));
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            this.TableAlias = tableAlias;
            this.ColumnName = columnName;
            this.Alias = alias;
        }

        /// <summary>
        /// Gets the alias of the table containing the column.
        /// </summary>
        public string TableAlias { get; }

        /// <summary>
        /// Gets the name of the column to select.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the optional alias for the selected column in the result set.
        /// </summary>
        public string Alias { get; }
    }
}

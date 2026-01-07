// <copyright file="TemporaryTableJoinDefinition.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a join condition between a main table and a temporary table.
    /// </summary>
    public class TemporaryTableJoinDefinition
    {
        private readonly List<(string MainTableColumn, string TempTableColumn)> joinConditions;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryTableJoinDefinition"/> class.
        /// </summary>
        /// <param name="mainTableName">The name of the main table to join with.</param>
        /// <param name="tempTableName">The name of the temporary table to join with.</param>
        /// <param name="mainTableAlias">The alias for the main table in the SQL query.</param>
        /// <param name="tempTableAlias">The alias for the temporary table in the SQL query.</param>
        public TemporaryTableJoinDefinition(
            string mainTableName,
            string tempTableName,
            string mainTableAlias,
            string tempTableAlias)
        {
            if (string.IsNullOrWhiteSpace(mainTableName))
            {
                throw new ArgumentNullException(nameof(mainTableName));
            }

            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            if (string.IsNullOrWhiteSpace(mainTableAlias))
            {
                throw new ArgumentNullException(nameof(mainTableAlias));
            }

            if (string.IsNullOrWhiteSpace(tempTableAlias))
            {
                throw new ArgumentNullException(nameof(tempTableAlias));
            }

            this.MainTableName = mainTableName;
            this.TempTableName = tempTableName;
            this.MainTableAlias = mainTableAlias;
            this.TempTableAlias = tempTableAlias;
            this.joinConditions = [];
        }

        /// <summary>
        /// Gets the name of the main table to join with.
        /// </summary>
        public string MainTableName { get; }

        /// <summary>
        /// Gets the name of the temporary table to join with.
        /// </summary>
        public string TempTableName { get; }

        /// <summary>
        /// Gets the alias for the main table in the SQL query.
        /// </summary>
        public string MainTableAlias { get; }

        /// <summary>
        /// Gets the alias for the temporary table in the SQL query.
        /// </summary>
        public string TempTableAlias { get; }

        /// <summary>
        /// Gets the join conditions (read-only collection of main table column and temp table column pairs).
        /// </summary>
        public IReadOnlyList<(string MainTableColumn, string TempTableColumn)> JoinConditions =>
            this.joinConditions.AsReadOnly();

        /// <summary>
        /// Adds a join condition between a column in the main table and a column in the temporary table.
        /// </summary>
        /// <param name="mainTableColumn">The column name in the main table.</param>
        /// <param name="tempTableColumn">The column name in the temporary table.</param>
        /// <returns>The current instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">If column names are null or empty.</exception>
        public TemporaryTableJoinDefinition AddJoinCondition(string mainTableColumn, string tempTableColumn)
        {
            if (string.IsNullOrWhiteSpace(mainTableColumn))
            {
                throw new ArgumentNullException(nameof(mainTableColumn));
            }

            if (string.IsNullOrWhiteSpace(tempTableColumn))
            {
                throw new ArgumentNullException(nameof(tempTableColumn));
            }

            this.joinConditions.Add((mainTableColumn, tempTableColumn));
            return this;
        }

        /// <summary>
        /// Validates that at least one join condition is defined.
        /// </summary>
        /// <exception cref="InvalidOperationException">If no join conditions are defined.</exception>
        public void Validate()
        {
            if (this.joinConditions.Count == 0)
            {
                throw new InvalidOperationException("At least one join condition must be defined.");
            }
        }
    }
}

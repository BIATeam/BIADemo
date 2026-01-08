// <copyright file="TemporaryTableProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers.TemporaryTable
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract base class for managing temporary tables in database operations.
    /// Provides common logic while delegating provider-specific SQL syntax to subclasses.
    /// </summary>
    internal abstract class TemporaryTableProvider
    {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
        /// <summary>
        /// Gets the identifier quote character for the specific database provider (e.g., '[' for SQL Server, '"' for PostgreSQL).
        /// </summary>
        protected abstract char IdentifierQuoteOpen { get; }

        /// <summary>
        /// Gets the identifier quote close character for the specific database provider (e.g., ']' for SQL Server, '"' for PostgreSQL).
        /// </summary>
        protected abstract char IdentifierQuoteClose { get; }

        /// <summary>
        /// Gets the temporary table prefix for the specific database provider (e.g., '#' for SQL Server, empty for PostgreSQL).
        /// </summary>
        protected abstract string TempTablePrefix { get; }

        /// <summary>
        /// Gets a value indicating whether the database provider requires the TEMPORARY keyword in CREATE TABLE statements.
        /// </summary>
        protected abstract bool RequiresTemporaryKeyword { get; }

        /// <summary>
        /// Gets a value indicating whether the database provider supports IF EXISTS in DROP TABLE statements.
        /// </summary>
        protected abstract bool SupportsIfExists { get; }

        /// <summary>
        /// Retrieves data from a temporary table using a custom select query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selectQuery">The SELECT query to execute. Must return rows from the temporary table.</param>
        /// <param name="resultSelector">Function to map query results to TResult.</param>
        /// <param name="connection">The database connection.</param>
        /// <returns>A list of results.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        public static async Task<List<TResult>> GetDataFromTemporaryTableAsync<TResult>(
            string selectQuery,
            Func<DbDataReader, Task<TResult>> resultSelector,
            DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(selectQuery))
            {
                throw new ArgumentNullException(nameof(selectQuery));
            }

            ArgumentNullException.ThrowIfNull(resultSelector);

            ArgumentNullException.ThrowIfNull(connection);

            await EnsureConnectionOpenedAsync(connection);

            var results = new List<TResult>();

            using var command = connection.CreateCommand();
            command.CommandText = selectQuery;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(await resultSelector(reader));
            }

            return results;
        }

        /// <summary>
        /// Creates a temporary table and inserts values based on the temporary table definition.
        /// </summary>
        /// <typeparam name="TValue">The type of values to insert.</typeparam>
        /// <param name="tempTable">The temporary table definition containing schema and insertion configuration.</param>
        /// <param name="values">The list of values to insert.</param>
        /// <param name="connection">The database connection.</param>
        /// <param name="insertBatchSize">The number of values to insert per batch. Default value is 100.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        /// <exception cref="InvalidOperationException">If the temporary table definition is invalid.</exception>
        public async Task CreateAndPopulateTemporaryTableAsync<TValue>(
            TemporaryTable tempTable,
            IReadOnlyList<TValue> values,
            DbConnection connection,
            int insertBatchSize = 100)
        {
            ArgumentNullException.ThrowIfNull(tempTable);
            ArgumentNullException.ThrowIfNull(values);
            ArgumentNullException.ThrowIfNull(connection);

            // Create the table
            await this.CreateTemporaryTableAsync(
                tempTable.Name,
                tempTable.Columns,
                connection);

            // Populate the table - get insert columns in table column order
            var insertColumns = tempTable.Columns
                .Select(c => tempTable.GetInsertColumn<TValue>(c.Name))
                .ToList();

            await this.InsertValuesInTemporaryTableAsync(
                tempTable.Name,
                values,
                insertColumns,
                connection,
                insertBatchSize);
        }

        /// <summary>
        /// Builds a SELECT query that joins a main table with a temporary table based on defined join conditions.
        /// </summary>
        /// <param name="joinDefinition">The join definition specifying tables, aliases, and join conditions.</param>
        /// <param name="selectColumns">The columns to select from the joined tables.</param>
        /// <returns>The SELECT query string.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        /// <exception cref="ArgumentException">If selectColumns is empty.</exception>
        public string BuildSelectFromTemporaryTableJoin(
            TemporaryTableJoinDefinition joinDefinition,
            params TemporaryTableSelectColumn[] selectColumns)
        {
            ArgumentNullException.ThrowIfNull(joinDefinition);
            ArgumentNullException.ThrowIfNull(selectColumns);

            if (selectColumns.Length == 0)
            {
                throw new ArgumentException("At least one select column must be specified.", nameof(selectColumns));
            }

            joinDefinition.Validate();

            return this.GetSelectFromTemporaryTableJoinSql(joinDefinition, selectColumns);
        }

        /// <summary>
        /// Drops the temporary table from the database.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="connection">The database connection.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        public async Task DropTemporaryTableAsync(
            string tempTableName,
            DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            ArgumentNullException.ThrowIfNull(connection);

            await EnsureConnectionOpenedAsync(connection);

            var dropTableSql = this.GetDropTableSql(tempTableName);

            using var command = connection.CreateCommand();
            command.CommandText = dropTableSql;
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Creates a parameter for the specific database provider.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A DbParameter instance.</returns>
        protected abstract DbParameter CreateParameter(string parameterName, object value);

        /// <summary>
        /// Quotes an identifier (table name, column name) according to the database provider's syntax.
        /// </summary>
        /// <param name="identifier">The identifier to quote.</param>
        /// <returns>The quoted identifier.</returns>
        protected string QuoteIdentifier(string identifier)
        {
            return $"{this.IdentifierQuoteOpen}{identifier}{this.IdentifierQuoteClose}";
        }

        /// <summary>
        /// Formats a temporary table name with the provider-specific prefix.
        /// </summary>
        /// <param name="tempTableName">The temporary table name.</param>
        /// <returns>The formatted table name with prefix.</returns>
        protected string FormatTempTableName(string tempTableName)
        {
            return $"{this.TempTablePrefix}{tempTableName}";
        }

        /// <summary>
        /// Gets the SQL syntax for creating a temporary table for the specific database provider.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="columns">The column definitions.</param>
        /// <returns>The CREATE TABLE SQL statement.</returns>
        protected virtual string GetCreateTableSql(
            string tempTableName,
            IReadOnlyList<TemporaryTableColumnDefinition> columns)
        {
            var columnDefinitions = columns.Select(col =>
            {
                var nullable = col.IsNullable ? "NULL" : "NOT NULL";
                var primaryKey = col.IsPrimaryKey ? "PRIMARY KEY" : string.Empty;
                var parts = new[] { this.QuoteIdentifier(col.Name), col.SqlType, nullable, primaryKey }
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .ToList();
                return string.Join(" ", parts);
            });

            var columnList = string.Join(",\n    ", columnDefinitions);
            var temporaryKeyword = this.RequiresTemporaryKeyword ? "TEMPORARY " : string.Empty;
            var tableName = this.QuoteIdentifier(this.FormatTempTableName(tempTableName));

            return $@"CREATE {temporaryKeyword}TABLE {tableName} (
    {columnList}
);";
        }

        /// <summary>
        /// Gets the SQL syntax for dropping a temporary table for the specific database provider.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <returns>The DROP TABLE SQL statement.</returns>
        protected virtual string GetDropTableSql(string tempTableName)
        {
            var ifExists = this.SupportsIfExists ? "IF EXISTS " : string.Empty;
            var tableName = this.QuoteIdentifier(this.FormatTempTableName(tempTableName));
            return $"DROP TABLE {ifExists}{tableName}";
        }

        /// <summary>
        /// Gets the SQL syntax for inserting values into a temporary table for the specific database provider.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="columnNames">The column names to insert values into.</param>
        /// <param name="valueCount">The number of rows to insert.</param>
        /// <returns>The INSERT SQL statement.</returns>
        protected virtual string GetInsertSql(
            string tempTableName,
            IReadOnlyList<string> columnNames,
            int valueCount)
        {
            var columns = string.Join($"{this.IdentifierQuoteClose}, {this.IdentifierQuoteOpen}", columnNames);
            var quotedColumns = $"{this.IdentifierQuoteOpen}{columns}{this.IdentifierQuoteClose}";

            var valuesList = string.Join(",", Enumerable.Range(0, valueCount)
                .Select(i => $"({string.Join(",", Enumerable.Range(0, columnNames.Count).Select(j => $"@Value{(i * columnNames.Count) + j}"))})"));

            var tableName = this.QuoteIdentifier(this.FormatTempTableName(tempTableName));
            return $@"INSERT INTO {tableName} ({quotedColumns}) VALUES {valuesList}";
        }

        /// <summary>
        /// Gets the SQL syntax for a select query that joins a table with a temporary table.
        /// </summary>
        /// <param name="joinDefinition">The join definition specifying tables, aliases, and join conditions.</param>
        /// <param name="selectColumns">The columns to select from the joined tables.</param>
        /// <returns>The SELECT query string.</returns>
        protected virtual string GetSelectFromTemporaryTableJoinSql(
            TemporaryTableJoinDefinition joinDefinition,
            TemporaryTableSelectColumn[] selectColumns)
        {
            var selectList = string.Join(", ", selectColumns.Select(col =>
            {
                var tableAlias = col.TableAlias == joinDefinition.TempTableAlias
                    ? joinDefinition.TempTableAlias
                    : joinDefinition.MainTableAlias;

                var columnRef = $"{this.QuoteIdentifier(tableAlias)}.{this.QuoteIdentifier(col.ColumnName)}";

                return string.IsNullOrWhiteSpace(col.Alias)
                    ? columnRef
                    : $"{columnRef} AS {this.QuoteIdentifier(col.Alias)}";
            }));

            var joinConditions = string.Join(" AND ", joinDefinition.JoinConditions.Select(jc =>
                $"{this.QuoteIdentifier(joinDefinition.MainTableAlias)}.{this.QuoteIdentifier(jc.MainTableColumn)} = {this.QuoteIdentifier(joinDefinition.TempTableAlias)}.{this.QuoteIdentifier(jc.TempTableColumn)}"));

            var mainTable = this.QuoteIdentifier(joinDefinition.MainTableName);
            var tempTable = this.QuoteIdentifier(this.FormatTempTableName(joinDefinition.TempTableName));
            var mainAlias = this.QuoteIdentifier(joinDefinition.MainTableAlias);
            var tempAlias = this.QuoteIdentifier(joinDefinition.TempTableAlias);

            return $@"
SELECT {selectList}
FROM {mainTable} {mainAlias}
INNER JOIN {tempTable} {tempAlias} ON {joinConditions}";
        }

        /// <summary>
        /// Ensures the database connection is open.
        /// </summary>
        /// <param name="connection">The database connection to check and open if necessary.</param>
        /// <returns>A completed task.</returns>
        private static async Task EnsureConnectionOpenedAsync(DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }

        /// <summary>
        /// Creates a temporary table with the specified column schema.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="columns">The column definitions.</param>
        /// <param name="connection">The database connection.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        /// <exception cref="ArgumentException">If columns list is empty.</exception>
        private async Task CreateTemporaryTableAsync(
            string tempTableName,
            IReadOnlyList<TemporaryTableColumnDefinition> columns,
            DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            ArgumentNullException.ThrowIfNull(columns);

            if (columns.Count == 0)
            {
                throw new ArgumentException("At least one column must be specified.", nameof(columns));
            }

            ArgumentNullException.ThrowIfNull(connection);

            await EnsureConnectionOpenedAsync(connection);

            var createTableSql = this.GetCreateTableSql(tempTableName, columns);

            using var command = connection.CreateCommand();
            command.CommandText = createTableSql;
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Inserts values into multiple columns of a temporary table in batches.
        /// </summary>
        /// <typeparam name="TValue">The type of values to insert.</typeparam>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="values">The list of values to insert.</param>
        /// <param name="insertColumns">The column definitions specifying which columns to insert and how to extract/format values.</param>
        /// <param name="connection">The database connection.</param>
        /// <param name="insertBatchSize">The number of values to insert per batch. Default value is 100.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        /// <exception cref="ArgumentException">If insertColumns is empty.</exception>
        private async Task InsertValuesInTemporaryTableAsync<TValue>(
            string tempTableName,
            IReadOnlyList<TValue> values,
            IReadOnlyList<TemporaryTableInsertColumn<TValue>> insertColumns,
            DbConnection connection,
            int insertBatchSize = 100)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            ArgumentNullException.ThrowIfNull(values);

            ArgumentNullException.ThrowIfNull(insertColumns);

            if (insertColumns.Count == 0)
            {
                throw new ArgumentException("At least one insert column must be specified.", nameof(insertColumns));
            }

            ArgumentNullException.ThrowIfNull(connection);

            await EnsureConnectionOpenedAsync(connection);

            var columnNames = insertColumns.Select(col => col.ColumnName).ToList();
            var insertBatches = values.Select((value, index) => new { value, index })
                .GroupBy(x => x.index / insertBatchSize)
                .ToList();

            foreach (var batch in insertBatches)
            {
                var batchValues = batch.Select(x => x.value).ToList();
                var insertSql = this.GetInsertSql(tempTableName, columnNames, batchValues.Count);

                using var command = connection.CreateCommand();
                command.CommandText = insertSql;

                for (int rowIndex = 0; rowIndex < batchValues.Count; rowIndex++)
                {
                    var rowValue = batchValues[rowIndex];

                    for (int colIndex = 0; colIndex < insertColumns.Count; colIndex++)
                    {
                        var insertColumn = insertColumns[colIndex];
                        var extractedValue = insertColumn.ValueSelector(rowValue);
                        var formattedValue = insertColumn.FormatValue?.Invoke(extractedValue);
                        var parameterValue = formattedValue ?? (!Equals(extractedValue, default) ? extractedValue : DBNull.Value);

                        var parameterName = $"@Value{(rowIndex * insertColumns.Count) + colIndex}";
                        command.Parameters.Add(this.CreateParameter(parameterName, parameterValue));
                    }
                }

                await command.ExecuteNonQueryAsync();
            }
        }
    }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
}

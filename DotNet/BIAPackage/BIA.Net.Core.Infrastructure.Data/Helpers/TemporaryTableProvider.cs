// <copyright file="TemporaryTableProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
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
    public abstract class TemporaryTableProvider
    {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
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
        /// Creates a temporary table with the specified single column schema.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="columnName">The column name to create in the temporary table.</param>
        /// <param name="columnType">The SQL type of the column.</param>
        /// <param name="connection">The database connection.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        public async Task CreateTemporaryTableSingleColumnAsync(
            string tempTableName,
            string columnName,
            string columnType,
            DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            if (string.IsNullOrWhiteSpace(columnType))
            {
                throw new ArgumentNullException(nameof(columnType));
            }

            ArgumentNullException.ThrowIfNull(connection);

            await EnsureConnectionOpenedAsync(connection);

            var createTableSql = this.GetCreateTableSql(tempTableName, columnName, columnType);

            using var command = connection.CreateCommand();
            command.CommandText = createTableSql;
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Inserts values into a temporary table's single column in batches.
        /// </summary>
        /// <typeparam name="TValue">The type of values to insert.</typeparam>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="values">The list of values to insert.</param>
        /// <param name="columnName">The column name to insert values into.</param>
        /// <param name="connection">The database connection.</param>
        /// <param name="formatValue">Optional function to format the value for SQL. If null, the value is used as-is.</param>
        /// <param name="insertBatchSize">The number of values to insert per batch. Default value is 100.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        public async Task InsertValuesInTemporaryTableSingleColumnAsync<TValue>(
            string tempTableName,
            IReadOnlyList<TValue> values,
            string columnName,
            DbConnection connection,
            Func<TValue, object> formatValue = null,
            int insertBatchSize = 100)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            ArgumentNullException.ThrowIfNull(values);

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            ArgumentNullException.ThrowIfNull(connection);

            await EnsureConnectionOpenedAsync(connection);

            var insertBatches = values.Select((value, index) => new { value, index })
                .GroupBy(x => x.index / insertBatchSize)
                .ToList();

            foreach (var batch in insertBatches)
            {
                var batchValues = batch.Select(x => x.value).ToList();
                var insertSql = this.GetInsertSql(tempTableName, columnName, batchValues.Count);

                using var command = connection.CreateCommand();
                command.CommandText = insertSql;

                for (int index = 0; index < batchValues.Count; index++)
                {
                    var value = batchValues[index];
                    var formattedValue = formatValue?.Invoke(value);
                    var parameterValue = formattedValue ?? (!Equals(value, default(TValue)) ? value : DBNull.Value);

                    command.Parameters.Add(this.CreateParameter($"@Value{index}", parameterValue));
                }

                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Builds a SELECT query that joins a main table with a temporary table containing a single column.
        /// </summary>
        /// <param name="tableName">The name of the main table to select from.</param>
        /// <param name="tableJoinColumnName">The column name in the main table to join on.</param>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="tempTableColumnName">The column name in the temporary table.</param>
        /// <param name="selectColumns">The columns to select in format "tableAlias.[columnName]".</param>
        /// <returns>The SELECT query string.</returns>
        public string BuildSelectFromTemporaryTableSingleColumn(
            string tableName,
            string tableJoinColumnName,
            string tempTableName,
            string tempTableColumnName,
            params string[] selectColumns)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (string.IsNullOrWhiteSpace(tableJoinColumnName))
            {
                throw new ArgumentNullException(nameof(tableJoinColumnName));
            }

            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            if (string.IsNullOrWhiteSpace(tempTableColumnName))
            {
                throw new ArgumentNullException(nameof(tempTableColumnName));
            }

            ArgumentNullException.ThrowIfNull(selectColumns);

            if (selectColumns.Length == 0)
            {
                throw new ArgumentException("At least one select column must be specified.", nameof(selectColumns));
            }

            return this.GetSelectFromTemporaryTableSingleColumnSql(
                tableName,
                tableJoinColumnName,
                tempTableName,
                tempTableColumnName,
                selectColumns);
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
        /// Gets the SQL syntax for creating a temporary table for the specific database provider.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="columnName">The column name to create in the temporary table.</param>
        /// <param name="columnType">The SQL type of the column.</param>
        /// <returns>The CREATE TABLE SQL statement.</returns>
        protected abstract string GetCreateTableSql(string tempTableName, string columnName, string columnType);

        /// <summary>
        /// Gets the SQL syntax for dropping a temporary table for the specific database provider.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <returns>The DROP TABLE SQL statement.</returns>
        protected abstract string GetDropTableSql(string tempTableName);

        /// <summary>
        /// Gets the SQL syntax for inserting values into a temporary table for the specific database provider.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="columnName">The column name to insert values into.</param>
        /// <param name="valueCount">The number of parameter placeholders required.</param>
        /// <returns>The INSERT SQL statement.</returns>
        protected abstract string GetInsertSql(string tempTableName, string columnName, int valueCount);

        /// <summary>
        /// Creates a parameter for the specific database provider.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A DbParameter instance.</returns>
        protected abstract DbParameter CreateParameter(string parameterName, object value);

        /// <summary>
        /// Gets the SQL syntax for a select query that joins a table with a temporary table containing a single column.
        /// </summary>
        /// <param name="tableName">The name of the main table to select from.</param>
        /// <param name="tableJoinColumnName">The column name in the main table to join on.</param>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="tempTableColumnName">The column name in the temporary table.</param>
        /// <param name="selectColumns">The columns to select in format "tableAlias.[columnName]".</param>
        /// <returns>The SELECT query string.</returns>
        protected abstract string GetSelectFromTemporaryTableSingleColumnSql(
            string tableName,
            string tableJoinColumnName,
            string tempTableName,
            string tempTableColumnName,
            params string[] selectColumns);

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
    }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
}

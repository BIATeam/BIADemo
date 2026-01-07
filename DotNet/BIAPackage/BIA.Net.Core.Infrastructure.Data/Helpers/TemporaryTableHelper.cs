// <copyright file="TemporaryTableHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Npgsql;

    /// <summary>
    /// Helper class for managing temporary tables in database operations.
    /// </summary>
    public static class TemporaryTableHelper
#pragma warning disable CA2100
    {
        /// <summary>
        /// Creates and populates a temporary table with values.
        /// </summary>
        /// <typeparam name="TValue">The type of values to insert.</typeparam>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="values">The list of values to insert.</param>
        /// <param name="columnName">The column name to create in the temporary table.</param>
        /// <param name="columnType">The SQL type of the column.</param>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <param name="formatValue">Optional function to format the value for SQL. If null, the value is used as-is.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        /// <exception cref="NotSupportedException">If the database provider is not supported.</exception>
        public static async Task CreateAndPopulateTemporaryTableAsync<TValue>(
            string tempTableName,
            IReadOnlyList<TValue> values,
            string columnName,
            string columnType,
            DbProvider dbProvider,
            System.Data.Common.DbConnection connection,
            Func<TValue, object> formatValue = null)
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

            if (string.IsNullOrWhiteSpace(columnType))
            {
                throw new ArgumentNullException(nameof(columnType));
            }

            ArgumentNullException.ThrowIfNull(connection);

            var createTableSql = dbProvider switch
            {
                DbProvider.SqlServer => $@"CREATE TABLE [#{tempTableName}] (
    [{columnName}] {columnType} NOT NULL PRIMARY KEY
);",
                DbProvider.PostGreSql => $@"CREATE TEMPORARY TABLE ""{tempTableName}"" (
    ""{columnName}"" {columnType} NOT NULL PRIMARY KEY
);",
                _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
            };

            using (var command = connection.CreateCommand())
            {
                command.CommandText = createTableSql;
                await command.ExecuteNonQueryAsync();
            }

            if (values.Count > 0)
            {
                await InsertValuesIntoColumnBatchesAsync(tempTableName, values, columnName, dbProvider, connection, formatValue);
            }
        }

        /// <summary>
        /// Retrieves data from a temporary table using a custom select query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selectQuery">The SELECT query to execute. Must return rows from the temporary table.</param>
        /// <param name="resultSelector">Function to map query results to TResult.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <returns>A list of results.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        public static async Task<List<TResult>> GetDataFromTemporaryTableAsync<TResult>(
            string selectQuery,
            Func<System.Data.Common.DbDataReader, Task<TResult>> resultSelector,
            System.Data.Common.DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(selectQuery))
            {
                throw new ArgumentNullException(nameof(selectQuery));
            }

            ArgumentNullException.ThrowIfNull(resultSelector);

            ArgumentNullException.ThrowIfNull(connection);

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
        /// Drops a temporary table from the database.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null.</exception>
        /// <exception cref="NotSupportedException">If the database provider is not supported.</exception>
        public static async Task DropTemporaryTableAsync(
            string tempTableName,
            DbProvider dbProvider,
            System.Data.Common.DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException(nameof(tempTableName));
            }

            ArgumentNullException.ThrowIfNull(connection);

            var dropTableSql = dbProvider switch
            {
                DbProvider.SqlServer => $"DROP TABLE [#{tempTableName}]",
                DbProvider.PostGreSql => $@"DROP TABLE IF EXISTS ""{tempTableName}""",
                _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
            };

            using var command = connection.CreateCommand();
            command.CommandText = dropTableSql;
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Inserts values into a temporary table's column in batches.
        /// </summary>
        /// <typeparam name="TValue">The type of values to insert.</typeparam>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="values">The list of values to insert.</param>
        /// <param name="columnName">The column name to insert values into.</param>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <param name="formatValue">Optional function to format the value for SQL. If null, the value is used as-is.</param>
        /// <param name="insertBatchSize">The number of values to insert per batch. Default value is 100.</param>
        /// <returns>A completed task.</returns>
        private static async Task InsertValuesIntoColumnBatchesAsync<TValue>(
            string tempTableName,
            IReadOnlyList<TValue> values,
            string columnName,
            DbProvider dbProvider,
            System.Data.Common.DbConnection connection,
            Func<TValue, object> formatValue,
            int insertBatchSize = 100)
        {
            var insertBatches = values.Select((value, index) => new { value, index })
                .GroupBy(x => x.index / insertBatchSize)
                .ToList();

            foreach (var batch in insertBatches)
            {
                var batchValues = batch.Select(x => x.value).ToList();
                var valuesList = string.Join(",", batchValues.Select((_, i) => $"(@Value{i})"));

                var insertSql = dbProvider switch
                {
                    DbProvider.SqlServer => $@"INSERT INTO [#{tempTableName}] ([{columnName}]) VALUES {valuesList}",
                    DbProvider.PostGreSql => $@"INSERT INTO ""{tempTableName}"" (""{columnName}"") VALUES {valuesList}",
                    _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
                };

                using var command = connection.CreateCommand();
                command.CommandText = insertSql;

                for (int index = 0; index < batchValues.Count; index++)
                {
                    var value = batchValues[index];
                    var formattedValue = formatValue?.Invoke(value);
                    var parameterValue = formattedValue ?? (!Equals(value, default(TValue)) ? value : DBNull.Value);

                    if (dbProvider == DbProvider.SqlServer)
                    {
                        command.Parameters.Add(new SqlParameter($"@Value{index}", parameterValue));
                    }
                    else if (dbProvider == DbProvider.PostGreSql)
                    {
                        command.Parameters.Add(new NpgsqlParameter($"@Value{index}", parameterValue));
                    }
                }

                await command.ExecuteNonQueryAsync();
            }
        }
#pragma warning restore CA2100
    }
}

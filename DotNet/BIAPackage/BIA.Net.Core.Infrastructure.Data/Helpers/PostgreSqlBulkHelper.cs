// <copyright file="PostgreSqlBulkHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Npgsql;
    using NpgsqlTypes;

    /// <summary>
    /// PostgreSQL Bulk Helper.
    /// </summary>
    public static class PostgreSqlBulkHelper
    {
        private const int MaxParametersPerCommand = 60000;

        /// <summary>
        /// Inserts the specified data using PostgreSQL COPY.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The data to insert.</param>
        /// <param name="bulkBatchSize">Number of rows processed per batch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task InsertAsync<T>(BiaDataContext dbContext, List<T> datas, int bulkBatchSize = 10_000)
            where T : class
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }

            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            IEntityType entityType = dbContext.Model.FindEntityType(typeof(T));
            string connectionString = dbContext.Database.GetConnectionString();
            string tableName = entityType.GetTableName();
            string schema = entityType.GetSchema();

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            List<ColumnMapping> columnMappings = BuildInsertMappings<T>(entityType);
            if (columnMappings.Count == 0)
            {
                return;
            }

            string qualifiedTableName = BuildQualifiedTableName(schema, tableName);
            string columnList = string.Join(", ", columnMappings.Select(mapping => QuoteIdentifier(mapping.SqlName)));

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                foreach (List<T> chunk in SplitIntoChunks(datas, bulkBatchSize))
                {
                    string copyCommand = string.Format("COPY {0} ({1}) FROM STDIN (FORMAT BINARY)", qualifiedTableName, columnList);
                    // using (var importer = connection.BeginBinaryImport(copyCommand))
                    using (var importer = await connection.BeginBinaryImportAsync(copyCommand).ConfigureAwait(false))
                    {
                        foreach (T item in chunk)
                        {
                            await importer.StartRowAsync().ConfigureAwait(false);
                            foreach (ColumnMapping mapping in columnMappings)
                            {
                                object value = mapping.PropertyInfo.GetValue(item, null);
                                if (value == null)
                                {
                                    await importer.WriteNullAsync().ConfigureAwait(false);
                                }
                                else
                                {
                                    if (mapping.NpgsqlDbType.HasValue)
                                    {
                                        await importer.WriteAsync(value, mapping.NpgsqlDbType.Value).ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        await importer.WriteAsync(value).ConfigureAwait(false);
                                    }
                                }
                            }
                        }

                        await importer.CompleteAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the specified data using PostgreSQL upsert.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The data to update.</param>
        /// <param name="bulkBatchSize">Number of rows processed per batch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task UpdateAsync<T>(BiaDataContext dbContext, List<T> datas, int bulkBatchSize = 10_000)
            where T : class
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }

            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            IEntityType entityType = dbContext.Model.FindEntityType(typeof(T));
            string connectionString = dbContext.Database.GetConnectionString();
            string tableName = entityType.GetTableName();
            string schema = entityType.GetSchema();

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            List<ColumnMapping> columnMappings = BuildUpdateMappings<T>(entityType);
            if (columnMappings.Count == 0)
            {
                return;
            }

            ColumnMapping primaryKeyMapping = GetPrimaryKeyMapping<T>(entityType);
            if (primaryKeyMapping == null)
            {
                throw new InvalidOperationException(string.Format("No primary key found for entity type {0}", typeof(T).Name));
            }

            string qualifiedTableName = BuildQualifiedTableName(schema, tableName);
            string columnList = string.Join(", ", columnMappings.Select(mapping => QuoteIdentifier(mapping.SqlName)));
            string conflictTarget = QuoteIdentifier(primaryKeyMapping.SqlName);
            string updateClause = string.Join(", ", columnMappings.Where(mapping => !string.Equals(mapping.SqlName, primaryKeyMapping.SqlName, StringComparison.Ordinal)).Select(mapping => string.Format("{0} = EXCLUDED.{0}", QuoteIdentifier(mapping.SqlName))));

            if (string.IsNullOrWhiteSpace(updateClause))
            {
                return;
            }

            int parametersPerRow = columnMappings.Count;
            int maxRowsPerCommand = Math.Max(1, MaxParametersPerCommand / Math.Max(1, parametersPerRow));
            int effectiveBatchSize = bulkBatchSize > 0 ? Math.Min(bulkBatchSize, maxRowsPerCommand) : maxRowsPerCommand;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                foreach (List<T> chunk in SplitIntoChunks(datas, effectiveBatchSize))
                {
                    using (var command = connection.CreateCommand())
                    {
                        StringBuilder valuesBuilder = new StringBuilder();
                        int parameterIndex = 0;

                        for (int rowIndex = 0; rowIndex < chunk.Count; rowIndex++)
                        {
                            if (rowIndex > 0)
                            {
                                valuesBuilder.Append(", ");
                            }

                            valuesBuilder.Append("(");

                            for (int columnIndex = 0; columnIndex < columnMappings.Count; columnIndex++)
                            {
                                if (columnIndex > 0)
                                {
                                    valuesBuilder.Append(", ");
                                }

                                string parameterName = string.Format("@p{0}", parameterIndex++);
                                valuesBuilder.Append(parameterName);

                                object value = columnMappings[columnIndex].PropertyInfo.GetValue(chunk[rowIndex], null) ?? (object)DBNull.Value;
                                command.Parameters.AddWithValue(parameterName, value);
                            }

                            valuesBuilder.Append(")");
                        }

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        command.CommandText = string.Format(
                            "INSERT INTO {0} ({1}) VALUES {2} ON CONFLICT ({3}) DO UPDATE SET {4}",
                            qualifiedTableName,
                            columnList,
                            valuesBuilder.ToString(),
                            conflictTarget,
                            updateClause);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the specified data using PostgreSQL delete with VALUES.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The data to delete.</param>
        /// <param name="bulkBatchSize">Number of rows processed per batch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task DeleteAsync<T>(BiaDataContext dbContext, List<T> datas, int bulkBatchSize = 10_000)
            where T : class
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }

            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            IEntityType entityType = dbContext.Model.FindEntityType(typeof(T));
            string connectionString = dbContext.Database.GetConnectionString();
            string tableName = entityType.GetTableName();
            string schema = entityType.GetSchema();

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            ColumnMapping primaryKeyMapping = GetPrimaryKeyMapping<T>(entityType);
            if (primaryKeyMapping == null)
            {
                throw new InvalidOperationException(string.Format("No primary key found for entity type {0}", typeof(T).Name));
            }

            string qualifiedTableName = BuildQualifiedTableName(schema, tableName);
            string primaryKeyColumn = QuoteIdentifier(primaryKeyMapping.SqlName);

            int parametersPerRow = 1;
            int maxRowsPerCommand = Math.Max(1, MaxParametersPerCommand / parametersPerRow);
            int effectiveBatchSize = bulkBatchSize > 0 ? Math.Min(bulkBatchSize, maxRowsPerCommand) : maxRowsPerCommand;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                foreach (List<T> chunk in SplitIntoChunks(datas, effectiveBatchSize))
                {
                    using (var command = connection.CreateCommand())
                    {
                        StringBuilder valuesBuilder = new StringBuilder();
                        int parameterIndex = 0;

                        for (int rowIndex = 0; rowIndex < chunk.Count; rowIndex++)
                        {
                            if (rowIndex > 0)
                            {
                                valuesBuilder.Append(", ");
                            }

                            string parameterName = string.Format("@p{0}", parameterIndex++);
                            valuesBuilder.AppendFormat("({0})", parameterName);

                            object value = primaryKeyMapping.PropertyInfo.GetValue(chunk[rowIndex], null) ?? (object)DBNull.Value;
                            command.Parameters.AddWithValue(parameterName, value);
                        }

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        command.CommandText = string.Format(
                            "DELETE FROM {0} AS target USING (VALUES {1}) AS source({2}) WHERE target.{2} = source.{2}",
                            qualifiedTableName,
                            valuesBuilder.ToString(),
                            primaryKeyColumn);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        private static List<ColumnMapping> BuildInsertMappings<T>(IEntityType entityType)
            where T : class
        {
            List<ColumnMapping> mappings = new List<ColumnMapping>();
            foreach (IProperty property in entityType.GetProperties())
            {
                if (!CanWriteOnInsert(property))
                {
                    continue;
                }

                ColumnMapping mapping = CreateColumnMapping<T>(property);
                if (mapping != null)
                {
                    mappings.Add(mapping);
                }
            }

            return mappings;
        }

        private static List<ColumnMapping> BuildUpdateMappings<T>(IEntityType entityType)
            where T : class
        {
            List<ColumnMapping> mappings = new List<ColumnMapping>();
            foreach (IProperty property in entityType.GetProperties())
            {
                if (!CanWriteOnUpdate(property))
                {
                    continue;
                }

                ColumnMapping mapping = CreateColumnMapping<T>(property);
                if (mapping != null)
                {
                    mappings.Add(mapping);
                }
            }

            return mappings;
        }

        private static ColumnMapping GetPrimaryKeyMapping<T>(IEntityType entityType)
            where T : class
        {
            IProperty primaryKeyProperty = entityType.GetProperties().FirstOrDefault(p => p.IsPrimaryKey());
            if (primaryKeyProperty == null)
            {
                return null;
            }

            string columnName = primaryKeyProperty.GetColumnName();
            if (string.IsNullOrWhiteSpace(columnName))
            {
                return null;
            }

            ColumnMapping mapping = CreateColumnMapping<T>(primaryKeyProperty);
            return mapping;
        }

        private static ColumnMapping CreateColumnMapping<T>(IProperty property)
            where T : class
        {
            string columnName = property.GetColumnName();
            if (string.IsNullOrWhiteSpace(columnName))
            {
                return null;
            }

            PropertyInfo propertyInfo = property.PropertyInfo;
            if (propertyInfo == null)
            {
                propertyInfo = typeof(T).GetProperty(property.Name);
            }

            if (propertyInfo == null)
            {
                return null;
            }

            NpgsqlDbType? dbType = GetNpgsqlDbType(property);

            return new ColumnMapping
            {
                EntityName = property.Name,
                SqlName = columnName,
                PropertyInfo = propertyInfo,
                NpgsqlDbType = dbType,
            };
        }

        private static NpgsqlDbType? GetNpgsqlDbType(IProperty property)
        {
            string columnType = property.GetColumnType();
            if (!string.IsNullOrWhiteSpace(columnType))
            {
                string normalized = columnType.ToLowerInvariant().Split('(')[0].Trim();
                switch (normalized)
                {
                    case "smallint":
                        return NpgsqlDbType.Smallint;
                    case "integer":
                    case "int":
                        return NpgsqlDbType.Integer;
                    case "bigint":
                        return NpgsqlDbType.Bigint;
                    case "numeric":
                    case "decimal":
                        return NpgsqlDbType.Numeric;
                    case "real":
                        return NpgsqlDbType.Real;
                    case "double precision":
                    case "float":
                        return NpgsqlDbType.Double;
                    case "money":
                        return NpgsqlDbType.Money;
                    case "boolean":
                    case "bool":
                        return NpgsqlDbType.Boolean;
                    case "timestamp without time zone":
                    case "timestamp":
                        return NpgsqlDbType.Timestamp;
                    case "timestamp with time zone":
                        return NpgsqlDbType.TimestampTz;
                    case "date":
                        return NpgsqlDbType.Date;
                    case "time without time zone":
                    case "time":
                        return NpgsqlDbType.Time;
                    case "time with time zone":
                        return NpgsqlDbType.TimeTz;
                    case "bytea":
                        return NpgsqlDbType.Bytea;
                    case "uuid":
                        return NpgsqlDbType.Uuid;
                    case "json":
                        return NpgsqlDbType.Json;
                    case "jsonb":
                        return NpgsqlDbType.Jsonb;
                    case "text":
                    case "character varying":
                    case "varchar":
                    case "character":
                    case "char":
                        return NpgsqlDbType.Text;
                    case "interval":
                        return NpgsqlDbType.Interval;
                }
            }

            Type clrType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;

            if (clrType == typeof(int))
            {
                return NpgsqlDbType.Integer;
            }

            if (clrType == typeof(short))
            {
                return NpgsqlDbType.Smallint;
            }

            if (clrType == typeof(long))
            {
                return NpgsqlDbType.Bigint;
            }

            if (clrType == typeof(decimal))
            {
                return NpgsqlDbType.Numeric;
            }

            if (clrType == typeof(double))
            {
                return NpgsqlDbType.Double;
            }

            if (clrType == typeof(float))
            {
                return NpgsqlDbType.Real;
            }

            if (clrType == typeof(bool))
            {
                return NpgsqlDbType.Boolean;
            }

            if (clrType == typeof(DateTime))
            {
                return NpgsqlDbType.Timestamp;
            }

            if (clrType == typeof(TimeSpan))
            {
                return NpgsqlDbType.Interval;
            }

            if (clrType == typeof(Guid))
            {
                return NpgsqlDbType.Uuid;
            }

            if (clrType == typeof(byte[]))
            {
                return NpgsqlDbType.Bytea;
            }

            if (clrType == typeof(string))
            {
                return NpgsqlDbType.Text;
            }

            return null;
        }

        private static bool CanWriteOnInsert(IProperty property)
        {
            string columnName = property.GetColumnName();
            if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(VersionedTable.RowVersion))
            {
                return false;
            }

            if (property.ValueGenerated == ValueGenerated.OnAdd || property.ValueGenerated == ValueGenerated.OnAddOrUpdate || property.ValueGenerated == ValueGenerated.OnUpdate)
            {
                if (property.IsPrimaryKey())
                {
                    return false;
                }

                return false;
            }

            return true;
        }

        private static bool CanWriteOnUpdate(IProperty property)
        {
            string columnName = property.GetColumnName();
            if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(VersionedTable.RowVersion))
            {
                return false;
            }

            if (property.ValueGenerated == ValueGenerated.OnAddOrUpdate || property.ValueGenerated == ValueGenerated.OnUpdate)
            {
                return false;
            }

            return true;
        }

        private static string BuildQualifiedTableName(string schema, string tableName)
        {
            if (string.IsNullOrWhiteSpace(schema))
            {
                return QuoteIdentifier(tableName);
            }

            return string.Format("{0}.{1}", QuoteIdentifier(schema), QuoteIdentifier(tableName));
        }

        private static string QuoteIdentifier(string identifier)
        {
            return string.Format("\"{0}\"", identifier.Replace("\"", "\"\""));
        }

        private static IEnumerable<List<T>> SplitIntoChunks<T>(List<T> source, int chunkSize)
        {
            if (source == null || source.Count == 0)
            {
                yield break;
            }

            if (chunkSize <= 0 || chunkSize >= source.Count)
            {
                yield return new List<T>(source);
                yield break;
            }

            for (int index = 0; index < source.Count; index += chunkSize)
            {
                int length = Math.Min(chunkSize, source.Count - index);
                List<T> chunk = new List<T>(length);
                for (int offset = 0; offset < length; offset++)
                {
                    chunk.Add(source[index + offset]);
                }

                yield return chunk;
            }
        }

        private sealed class ColumnMapping
        {
            public string EntityName { get; set; }

            public string SqlName { get; set; }

            public PropertyInfo PropertyInfo { get; set; }

            public NpgsqlDbType? NpgsqlDbType { get; set; }
        }
    }
}

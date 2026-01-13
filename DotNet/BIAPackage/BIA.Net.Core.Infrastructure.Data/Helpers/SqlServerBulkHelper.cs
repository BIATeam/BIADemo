// <copyright file="SqlServerBulkHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// SqlServer Bulk Helper.
    /// </summary>
    public static class SqlServerBulkHelper
    {
        /// <summary>
        /// Inserts the specified datas.
        /// </summary>
        /// <typeparam name="T">Type en entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The datas.</param>
        /// <param name="bulkBatchSize">Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task InsertAsync<T>(BiaDataContext dbContext, List<T> datas, int bulkBatchSize = 10_000)
            where T : class
        {
            if (datas?.Count == 0)
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

            if (datas?.Count > 0 && !string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(tableName))
            {
                DataTable table = new DataTable();

                List<(string EntityName, string SqlName)> columnMappings = new List<(string EntityName, string SqlName)>();

                foreach (IProperty property in entityType.GetProperties())
                {
                    string entityName = property.Name;
                    string sqlName = property.GetColumnName();

                    if (!string.IsNullOrWhiteSpace(sqlName) && sqlName != nameof(VersionedTable.RowVersion))
                    {
                        table.Columns.Add(sqlName);
                        columnMappings.Add((EntityName: entityName, SqlName: sqlName));
                    }
                }

                foreach (T item in datas)
                {
                    DataRow row = table.NewRow();

                    foreach ((string EntityName, string SqlName) mapping in columnMappings)
                    {
                        object value = item.GetType().GetProperty(mapping.EntityName)?.GetValue(item);
                        row[mapping.SqlName] = value ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = tableName;

                        if (bulkBatchSize > 0)
                        {
                            bulkCopy.BatchSize = bulkBatchSize;
                        }

                        foreach (string sqlName in columnMappings.Select(x => x.SqlName))
                        {
                            bulkCopy.ColumnMappings.Add(sqlName, sqlName);
                        }

                        await bulkCopy.WriteToServerAsync(table);
                    }

                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Updates the specified datas using bulk operations.
        /// </summary>
        /// <typeparam name="T">Type of entity that implements IEntity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The datas to update.</param>
        /// <param name="bulkBatchSize">Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task UpdateAsync<T>(BiaDataContext dbContext, List<T> datas, int bulkBatchSize = 10_000)
            where T : class
        {
            if (datas?.Count == 0)
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

            if (datas?.Count > 0 && !string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(tableName))
            {
                var columnMappings = new List<(string EntityName, string SqlName, string SqlType)>();
                var primaryKeyColumn = string.Empty;

                foreach (IProperty property in entityType.GetProperties())
                {
                    string entityName = property.Name;
                    string sqlName = property.GetColumnName();

                    if (!string.IsNullOrWhiteSpace(sqlName))
                    {
                        if (property.IsPrimaryKey())
                        {
                            primaryKeyColumn = sqlName;
                        }

                        if (sqlName == nameof(VersionedTable.RowVersion))
                        {
                            continue;
                        }

                        string sqlType = GetSqlType(property);
                        columnMappings.Add((EntityName: entityName, SqlName: sqlName, SqlType: sqlType));
                    }
                }

                if (string.IsNullOrWhiteSpace(primaryKeyColumn))
                {
                    throw new InvalidOperationException($"No primary key found for entity type {typeof(T).Name}");
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string tempTableName = $"#TempUpdate_{Guid.NewGuid():N}";

                    var createTableSql = new StringBuilder();
                    createTableSql.AppendLine($"CREATE TABLE {tempTableName} (");

                    var columnDefinitions = columnMappings.Select(col => $"    [{col.SqlName}] {col.SqlType}");
                    createTableSql.AppendLine(string.Join(",\n", columnDefinitions));
                    createTableSql.AppendLine(");");

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    using (var createCommand = new SqlCommand(createTableSql.ToString(), connection))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    {
                        await createCommand.ExecuteNonQueryAsync();
                    }

                    DataTable table = new DataTable();
                    foreach (var mapping in columnMappings)
                    {
                        table.Columns.Add(mapping.SqlName);
                    }

                    foreach (T item in datas)
                    {
                        DataRow row = table.NewRow();
                        foreach (var mapping in columnMappings)
                        {
                            object value = item.GetType().GetProperty(mapping.EntityName)?.GetValue(item);
                            row[mapping.SqlName] = value ?? DBNull.Value;
                        }

                        table.Rows.Add(row);
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = tempTableName;

                        if (bulkBatchSize > 0)
                        {
                            bulkCopy.BatchSize = bulkBatchSize;
                        }

                        foreach (string mappingSqlName in columnMappings.Select(mapping => mapping.SqlName))
                        {
                            bulkCopy.ColumnMappings.Add(mappingSqlName, mappingSqlName);
                        }

                        await bulkCopy.WriteToServerAsync(table);
                    }

                    var mergeSql = new StringBuilder();
                    mergeSql.AppendLine($"MERGE [{tableName}] AS target");
                    mergeSql.AppendLine($"USING {tempTableName} AS source");
                    mergeSql.AppendLine($"ON target.[{primaryKeyColumn}] = source.[{primaryKeyColumn}]");
                    mergeSql.AppendLine("WHEN MATCHED THEN");
                    mergeSql.AppendLine("    UPDATE SET");

                    var updateColumns = columnMappings
                        .Where(col => col.SqlName != primaryKeyColumn)
                        .Select(col => $"        target.[{col.SqlName}] = source.[{col.SqlName}]");
                    mergeSql.AppendLine(string.Join(",\n", updateColumns));
                    mergeSql.AppendLine(";");

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    using (var updateCommand = new SqlCommand(mergeSql.ToString(), connection))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    {
                        await updateCommand.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Deletes the specified entities using bulk operations.
        /// </summary>
        /// <typeparam name="T">Type of entity that implements IEntity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The data entities to delete.</param>
        /// <param name="bulkBatchSize">Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task DeleteAsync<T>(BiaDataContext dbContext, List<T> datas, int bulkBatchSize = 10_000)
            where T : class
        {
            if (datas?.Count == 0)
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

            if (datas?.Count > 0 && !string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(tableName))
            {
                string primaryKeyColumn = string.Empty;
                string primaryKeySqlType = string.Empty;
                string primaryKeyEntityName = string.Empty;

                IProperty property = entityType.GetProperties().FirstOrDefault(x => x.IsPrimaryKey());
                primaryKeyColumn = property.GetColumnName();
                primaryKeySqlType = GetSqlType(property);
                primaryKeyEntityName = property.Name;

                if (string.IsNullOrWhiteSpace(primaryKeyColumn))
                {
                    throw new InvalidOperationException($"No primary key found for entity type {typeof(T).Name}");
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string tempTableName = $"#TempDelete_{Guid.NewGuid():N}";

                    string createTableSql = $@"
                    CREATE TABLE {tempTableName} (
                        [{primaryKeyColumn}] {primaryKeySqlType}
                    );";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    using (var createCommand = new SqlCommand(createTableSql, connection))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    {
                        await createCommand.ExecuteNonQueryAsync();
                    }

                    DataTable table = new DataTable();
                    table.Columns.Add(primaryKeyColumn);

                    foreach (T item in datas)
                    {
                        DataRow row = table.NewRow();
                        object primaryKeyValue = item.GetType().GetProperty(primaryKeyEntityName)?.GetValue(item);
                        row[primaryKeyColumn] = primaryKeyValue ?? (object)DBNull.Value;
                        table.Rows.Add(row);
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = tempTableName;

                        if (bulkBatchSize > 0)
                        {
                            bulkCopy.BatchSize = bulkBatchSize;
                        }

                        bulkCopy.ColumnMappings.Add(primaryKeyColumn, primaryKeyColumn);
                        await bulkCopy.WriteToServerAsync(table);
                    }

                    string deleteSql = $@"
                    DELETE target 
                    FROM [{tableName}] AS target
                    INNER JOIN {tempTableName} AS source ON target.[{primaryKeyColumn}] = source.[{primaryKeyColumn}];";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    using (var deleteCommand = new SqlCommand(deleteSql, connection))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    {
                        await deleteCommand.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Gets the SQL type string for a given property.
        /// </summary>
        /// <param name="property">The entity property.</param>
        /// <returns>The SQL type string.</returns>
        private static string GetSqlType(IProperty property)
        {
            var columnType = property.GetColumnType();
            if (!string.IsNullOrWhiteSpace(columnType))
            {
                return columnType;
            }

            var clrType = property.ClrType;
            var underlyingType = Nullable.GetUnderlyingType(clrType) ?? clrType;

            if (clrType == typeof(byte[]))
            {
                return "varbinary(max)";
            }

            return underlyingType.Name switch
            {
                nameof(Int32) => "int",
                nameof(Int64) => "bigint",
                nameof(String) => property.GetMaxLength() is int maxLength and > 0 ? $"nvarchar({maxLength})" : "nvarchar(max)",
                nameof(DateTime) => "datetime2",
                nameof(Boolean) => "bit",
                nameof(Decimal) => GetDecimalType(property),
                nameof(Double) => "float",
                nameof(Single) => "real",
                nameof(Guid) => "uniqueidentifier",
                nameof(Byte) => "tinyint",
                nameof(Int16) => "smallint",
                nameof(DateTimeOffset) => "datetimeoffset",
                nameof(TimeSpan) => "time",
                nameof(SByte) => "tinyint",
                nameof(UInt16) => "int", // Promotion to prevent data loss
                nameof(UInt32) => "bigint", // Promotion to prevent data loss
                nameof(UInt64) => "decimal(20,0)", // Only viable option for very large values
                nameof(Char) => "nchar(1)",
                _ => "nvarchar(max)", // Secure fallback
            };
        }

        /// <summary>
        /// Gets the SQL decimal type with proper precision and scale.
        /// </summary>
        /// <param name="property">The entity property.</param>
        /// <returns>The SQL decimal type string.</returns>
        private static string GetDecimalType(IProperty property)
        {
            var precision = property.GetPrecision();
            var scale = property.GetScale();

            if (precision.HasValue && scale.HasValue)
            {
                return $"decimal({precision},{scale})";
            }
            else if (precision.HasValue)
            {
                return $"decimal({precision},0)";
            }

            return "decimal(18,2)"; // EF Core default value
        }
    }
}

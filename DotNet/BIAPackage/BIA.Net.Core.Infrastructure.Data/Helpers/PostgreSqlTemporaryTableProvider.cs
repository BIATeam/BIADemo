// <copyright file="PostgreSqlTemporaryTableProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System.Data.Common;
    using System.Linq;
    using Npgsql;

    /// <summary>
    /// Implementation of temporary table management for PostgreSQL.
    /// </summary>
    internal class PostgreSqlTemporaryTableProvider : TemporaryTableProvider
    {
        /// <inheritdoc/>
        protected override string GetCreateTableSql(string tempTableName, string columnName, string columnType)
        {
            return $@"CREATE TEMPORARY TABLE ""{tempTableName}"" (
    ""{columnName}"" {columnType} NOT NULL PRIMARY KEY
);";
        }

        /// <inheritdoc/>
        protected override string GetDropTableSql(string tempTableName)
        {
            return $@"DROP TABLE IF EXISTS ""{tempTableName}""";
        }

        /// <inheritdoc/>
        protected override string GetInsertSql(string tempTableName, string columnName, int valueCount)
        {
            var valuesList = string.Join(",", Enumerable.Range(0, valueCount).Select(i => $"(@Value{i})"));
            return $@"INSERT INTO ""{tempTableName}"" (""{columnName}"") VALUES {valuesList}";
        }

        /// <inheritdoc/>
        protected override DbParameter CreateParameter(string parameterName, object value)
        {
            return new NpgsqlParameter(parameterName, value);
        }

        /// <inheritdoc/>
        protected override string GetSelectFromTemporaryTableSingleColumnSql(
            string tableName,
            string tableJoinColumnName,
            string tempTableName,
            string tempTableColumnName,
            params string[] selectColumns)
        {
            var selectList = string.Join(", ", selectColumns.Select(x => $"t.\"{x}\""));
            return $@"
SELECT {selectList}
FROM ""{tableName}"" t
INNER JOIN ""{tempTableName}"" tt ON t.""{tableJoinColumnName}"" = tt.""{tempTableColumnName}""";
        }
    }
}

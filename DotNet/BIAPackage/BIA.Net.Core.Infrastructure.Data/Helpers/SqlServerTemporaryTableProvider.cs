// <copyright file="SqlServerTemporaryTableProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Implementation of temporary table management for SQL Server.
    /// </summary>
    internal class SqlServerTemporaryTableProvider : TemporaryTableProvider
    {
        /// <inheritdoc/>
        protected override string GetCreateTableSql(
            string tempTableName,
            IReadOnlyList<TemporaryTableColumnDefinition> columns)
        {
            var columnDefinitions = columns.Select(col =>
            {
                var nullable = col.IsNullable ? "NULL" : "NOT NULL";
                var primaryKey = col.IsPrimaryKey ? "PRIMARY KEY" : string.Empty;
                var parts = new[] { $"[{col.Name}]", col.SqlType, nullable, primaryKey }
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .ToList();
                return string.Join(" ", parts);
            });

            var columnList = string.Join(",\n    ", columnDefinitions);
            return $@"CREATE TABLE [#{tempTableName}] (
    {columnList}
);";
        }

        /// <inheritdoc/>
        protected override string GetDropTableSql(string tempTableName)
        {
            return $"DROP TABLE [#{tempTableName}]";
        }

        /// <inheritdoc/>
        protected override string GetInsertSql(
            string tempTableName,
            IReadOnlyList<string> columnNames,
            int valueCount)
        {
            var columns = string.Join("], [", columnNames);
            var valuesList = string.Join(",", Enumerable.Range(0, valueCount)
                .Select(i => $"({string.Join(",", Enumerable.Range(0, columnNames.Count).Select(j => $"@Value{(i * columnNames.Count) + j}"))})"));

            return $@"INSERT INTO [#{tempTableName}] ([{columns}]) VALUES {valuesList}";
        }

        /// <inheritdoc/>
        protected override DbParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }

        /// <inheritdoc/>
        protected override string GetSelectFromTemporaryTableJoinSql(
            TemporaryTableJoinDefinition joinDefinition,
            TemporaryTableSelectColumn[] selectColumns)
        {
            var selectList = string.Join(", ", selectColumns.Select(col =>
            {
                var columnRef = $"[{joinDefinition.MainTableAlias}].[{col.ColumnName}]";
                if (col.TableAlias == joinDefinition.TempTableAlias)
                {
                    columnRef = $"[{joinDefinition.TempTableAlias}].[{col.ColumnName}]";
                }

                return string.IsNullOrWhiteSpace(col.Alias)
                    ? columnRef
                    : $"{columnRef} AS [{col.Alias}]";
            }));

            var joinConditions = string.Join(" AND ", joinDefinition.JoinConditions.Select(jc =>
                $"[{joinDefinition.MainTableAlias}].[{jc.MainTableColumn}] = [{joinDefinition.TempTableAlias}].[{jc.TempTableColumn}]"));

            return $@"
SELECT {selectList}
FROM [{joinDefinition.MainTableName}] [{joinDefinition.MainTableAlias}]
INNER JOIN [#{joinDefinition.TempTableName}] [{joinDefinition.TempTableAlias}] ON {joinConditions}";
        }
    }
}

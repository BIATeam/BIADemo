// <copyright file="SqlInsertBulkHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// SqlServer Bulk Helper.
    /// </summary>
    internal static class SqlServerBulkHelper
    {
        /// <summary>
        /// Inserts the specified datas.
        /// </summary>
        /// <typeparam name="T">Type en entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datas">The datas.</param>
        public static void Insert<T>(BIADataContext dbContext, List<T> datas)
            where T : class
        {
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

                object tableLock = new object();

                Parallel.ForEach(datas, item =>
                {
                    DataRow row = default;

                    lock (tableLock)
                    {
                        row = table.NewRow();
                    }

                    foreach ((string EntityName, string SqlName) mapping in columnMappings)
                    {
                        object value = item.GetType().GetProperty(mapping.EntityName)?.GetValue(item);
                        row[mapping.SqlName] = value ?? DBNull.Value;
                    }

                    lock (tableLock)
                    {
                        table.Rows.Add(row);
                    }
                });

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = tableName;

                        foreach (string sqlName in columnMappings.Select(x => x.SqlName))
                        {
                            bulkCopy.ColumnMappings.Add(sqlName, sqlName);
                        }

                        bulkCopy.WriteToServer(table);
                    }

                    connection.Close();
                }
            }
        }
    }
}

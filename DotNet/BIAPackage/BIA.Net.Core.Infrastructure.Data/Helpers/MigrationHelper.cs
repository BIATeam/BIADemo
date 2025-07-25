﻿// <copyright file="MigrationHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// Migration Helper.
    /// </summary>
    public static class MigrationHelper
    {
        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="migrationBuilder">The migration builder.</param>
        /// <param name="sql">The SQL.</param>
        public static void ExecuteSql(MigrationBuilder migrationBuilder, string sql)
        {
            string executeSql = $"EXECUTE sp_executesql N'{sql}'";
            migrationBuilder.Sql(executeSql);
        }
    }
}

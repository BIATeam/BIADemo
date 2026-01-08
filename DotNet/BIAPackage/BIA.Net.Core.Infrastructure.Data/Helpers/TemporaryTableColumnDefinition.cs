// <copyright file="TemporaryTableColumnDefinition.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;

    /// <summary>
    /// Represents a column definition for a temporary table.
    /// </summary>
    internal class TemporaryTableColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryTableColumnDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="sqlType">The SQL type of the column (e.g., "NVARCHAR(MAX)", "INT", "UNIQUEIDENTIFIER").</param>
        /// <param name="isPrimaryKey">A value indicating whether the column is a primary key.</param>
        /// <param name="isNullable">A value indicating whether the column is nullable.</param>
        public TemporaryTableColumnDefinition(
            string name,
            string sqlType,
            bool isPrimaryKey = false,
            bool isNullable = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(sqlType))
            {
                throw new ArgumentNullException(nameof(sqlType));
            }

            this.Name = name;
            this.SqlType = sqlType;
            this.IsPrimaryKey = isPrimaryKey;
            this.IsNullable = isNullable;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the SQL type of the column (e.g., "NVARCHAR(MAX)", "INT", "UNIQUEIDENTIFIER").
        /// </summary>
        public string SqlType { get; }

        /// <summary>
        /// Gets a value indicating whether the column is a primary key.
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// Gets a value indicating whether the column is nullable.
        /// </summary>
        public bool IsNullable { get; }
    }
}

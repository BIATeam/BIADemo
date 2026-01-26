// <copyright file="PostgreSqlDateTimeConversionQuerySqlGenerator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Storage;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;

    /// <summary>
    /// Custom SQL query generator for PostgreSQL that knows how to render DateTimeFormatWithTimeZoneExpression.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage
    public class PostgreSqlDateTimeConversionQuerySqlGenerator : NpgsqlQuerySqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionQuerySqlGenerator"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        /// <param name="reverseNullOrderingEnabled">Whether reverse null ordering is enabled.</param>
        /// <param name="postgresVersion">The PostgreSQL version.</param>
        public PostgreSqlDateTimeConversionQuerySqlGenerator(
            QuerySqlGeneratorDependencies dependencies,
            IRelationalTypeMappingSource typeMappingSource,
            bool reverseNullOrderingEnabled,
            Version postgresVersion)
            : base(dependencies, typeMappingSource, reverseNullOrderingEnabled, postgresVersion)
        {
        }

        /// <summary>
        /// Visits a SQL expression and generates SQL for it.
        /// </summary>
        /// <param name="extensionExpression">The expression to visit.</param>
        /// <returns>The visited expression.</returns>
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            // Handle our custom DateTimeFormatWithTimeZoneExpression
            if (extensionExpression is DateTimeFormatWithTimeZoneExpression dateTimeFormat)
            {
                // PostgreSQL: TO_CHAR([column] AT TIME ZONE 'UTC' AT TIME ZONE [timezone], [format])
                this.Sql.Append("TO_CHAR(");
                this.Visit(dateTimeFormat.DateTimeColumn);
                this.Sql.Append(" AT TIME ZONE 'UTC' AT TIME ZONE ");

                // PostgreSQL uses IANA timezone names natively
                this.Visit(dateTimeFormat.TimeZoneId);

                this.Sql.Append(", ");
                this.Visit(dateTimeFormat.FormatString);
                this.Sql.Append(")");

                return extensionExpression;
            }

            // For all other expressions, use base implementation
            return base.VisitExtension(extensionExpression);
        }
    }
#pragma warning restore EF1001
}

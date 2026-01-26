// <copyright file="DateTimeConversionQuerySqlGenerator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.QueryExpression
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Custom SQL query generator for SQL Server that knows how to render DateTimeFormatWithTimeZoneExpression.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage
    public class SqlServerDateTimeConversionQuerySqlGenerator : SqlServerQuerySqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionQuerySqlGenerator"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        /// <param name="sqlServerOptions">The SQL Server options.</param>
        public SqlServerDateTimeConversionQuerySqlGenerator(
            QuerySqlGeneratorDependencies dependencies,
            IRelationalTypeMappingSource typeMappingSource,
            ISqlServerSingletonOptions sqlServerOptions)
            : base(dependencies, typeMappingSource, sqlServerOptions)
        {
        }

        /// <summary>
        /// Visits a SQL expression and generates SQL for it.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <returns>The visited expression.</returns>
        protected override Expression VisitExtension(Expression expression)
        {
            // Handle our custom DateTimeFormatWithTimeZoneExpression
            if (expression is DateTimeFormatWithTimeZoneExpression dateTimeFormat)
            {
                this.Sql.Append("FORMAT(");
                this.Visit(dateTimeFormat.DateTimeColumn);
                this.Sql.Append(" AT TIME ZONE N'UTC' AT TIME ZONE ");
                this.Visit(dateTimeFormat.TimeZoneId);
                this.Sql.Append(", ");
                this.Visit(dateTimeFormat.FormatString);
                this.Sql.Append(")");

                return expression;
            }

            // Handle legacy DateTimeAtTimeZoneExpression if needed
            if (expression is DateTimeAtTimeZoneExpression atTimeZone)
            {
                this.Sql.Append("(");
                this.Visit(atTimeZone.Operand);
                this.Sql.Append(" AT TIME ZONE ");
                this.Visit(atTimeZone.UtcTimeZone);
                this.Sql.Append(" AT TIME ZONE ");
                this.Visit(atTimeZone.TargetTimeZone);
                this.Sql.Append(")");

                return expression;
            }

            // For all other expressions, use base implementation
            return base.VisitExtension(expression);
        }
    }
#pragma warning restore EF1001
}

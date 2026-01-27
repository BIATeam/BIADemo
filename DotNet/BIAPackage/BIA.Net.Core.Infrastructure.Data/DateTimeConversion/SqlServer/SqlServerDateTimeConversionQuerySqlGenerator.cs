// <copyright file="SqlServerDateTimeConversionQuerySqlGenerator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.SqlServer
{
    using System.Linq.Expressions;
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Custom SQL query generator for SQL Server that knows how to render DateTimeFormatWithTimeZoneExpression.
    /// Uses CONVERT instead of FORMAT for significantly better performance (FORMAT uses CLR and is 10-50x slower).
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage
    internal sealed class SqlServerDateTimeConversionQuerySqlGenerator : SqlServerQuerySqlGenerator
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
        /// <param name="extensionExpression">The expression to visit.</param>
        /// <returns>The visited expression.</returns>
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            // Handle our custom DateTimeFormatWithTimeZoneExpression
            if (extensionExpression is DateTimeFormatWithTimeZoneExpression dateTimeFormat)
            {
                // Use CONVERT instead of FORMAT for better performance
                // FORMAT uses CLR and is much slower than native CONVERT
                // CONVERT style 120 produces: yyyy-mm-dd hh:mi:ss(24h)
                this.Sql.Append("CONVERT(varchar(19), CONVERT(datetime2(0), (");
                this.Visit(dateTimeFormat.DateTimeColumn);
                this.Sql.Append(" AT TIME ZONE N'UTC') AT TIME ZONE ");

                // The timezone should already be in Windows format from IClientTimeZoneContext.WindowsTimeZone.Id
                this.Visit(dateTimeFormat.TimeZoneId);

                this.Sql.Append("), 120)");

                return extensionExpression;
            }

            // For all other expressions, use base implementation
            return base.VisitExtension(extensionExpression);
        }
    }
#pragma warning restore EF1001
}

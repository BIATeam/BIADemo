// <copyright file="DateTimeConversionTranslator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using BIA.Net.Core.Common.Enum;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Handles all DateTime AT TIME ZONE translation logic for both SQL Server and PostgreSQL.
    /// EF Core will inject ISqlExpressionFactory and IRelationalTypeMappingSource automatically.
    /// </summary>
    public class DateTimeConversionTranslator : IMethodCallTranslatorPlugin, IMethodCallTranslator
    {
        private readonly ISqlExpressionFactory sqlExpressionFactory;
        private readonly IRelationalTypeMappingSource typeMappingSource;
        private readonly DbProvider dbProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="dbProvider">The database provider type.</param>
        public DateTimeConversionTranslator(
            IServiceProvider serviceProvider,
            DbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            this.sqlExpressionFactory = serviceProvider.GetRequiredService<ISqlExpressionFactory>();
            this.typeMappingSource = serviceProvider.GetRequiredService<IRelationalTypeMappingSource>();
        }

        /// <inheritdoc/>
        public IEnumerable<IMethodCallTranslator> Translators => new[] { this };

        /// <inheritdoc/>
        public SqlExpression Translate(
            SqlExpression instance,
            MethodInfo method,
            IReadOnlyList<SqlExpression> arguments,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            // Check if this is our ConvertDateTimeToLocalString method
            if (method.DeclaringType?.FullName != typeof(DatabaseDateTimeExpressionConverter).FullName
                || method.Name != nameof(DatabaseDateTimeExpressionConverter.ConvertDateTimeToLocalString)
                || arguments.Count != 2)
            {
                return null;
            }

            // arguments[0] = DateTime column
            // arguments[1] = timeZoneId string
            var dateTimeColumn = arguments[0];
            var timeZoneId = arguments[1];

            var stringTypeMapping = this.typeMappingSource.FindMapping(typeof(string));

            // Convert timezone format based on database provider
            SqlExpression processedTimeZone = null;
            if (this.dbProvider == DbProvider.SqlServer && timeZoneId is SqlConstantExpression constantTz && constantTz.Value is string tzValue)
            {
                // SQL Server needs Windows timezone - convert IANA to Windows if it's a constant
                if (TimeZoneInfo.TryConvertIanaIdToWindowsId(tzValue, out var windowsId))
                {
                    processedTimeZone = this.sqlExpressionFactory.Constant(windowsId, stringTypeMapping);
                }
                else
                {
                    // Already Windows format or conversion failed - use as-is
                    processedTimeZone = timeZoneId.TypeMapping == null
                        ? this.sqlExpressionFactory.ApplyTypeMapping(timeZoneId, stringTypeMapping)
                        : timeZoneId;
                }
            }
            else if (this.dbProvider == DbProvider.PostGreSql)
            {
                // PostgreSQL uses IANA natively, or it's a parameter (can't convert at translation time)
                processedTimeZone = timeZoneId.TypeMapping == null
                    ? this.sqlExpressionFactory.ApplyTypeMapping(timeZoneId, stringTypeMapping)
                    : timeZoneId;
            }

            // Create the complete expression
            // Note: The actual format is determined by the QuerySqlGenerator:
            //   - SQL Server: Uses CONVERT style 120 (yyyy-mm-dd hh:mi:ss 24h) - ignores this formatString
            //   - PostgreSQL: Uses TO_CHAR with this formatString
            var formatString = this.dbProvider switch
            {
                DbProvider.SqlServer => string.Empty, // Not used - CONVERT style 120 is hardcoded
                DbProvider.PostGreSql => "YYYY-MM-DD HH24:MI:SS",
                _ => throw new NotSupportedException($"The database provider '{this.dbProvider}' is not supported for DateTime conversion."),
            };

            return new DateTimeFormatWithTimeZoneExpression(
                dateTimeColumn,
                processedTimeZone,
                this.sqlExpressionFactory.Constant(formatString, stringTypeMapping),
                stringTypeMapping);
        }
    }
}

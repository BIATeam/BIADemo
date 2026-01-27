// <copyright file="SqlServerDateTimeConversionTranslator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.SqlServer
{
    using System;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// SQL Server-specific DateTime AT TIME ZONE translator.
    /// Converts IANA timezone IDs to Windows timezone names.
    /// </summary>
    internal sealed class SqlServerDateTimeConversionTranslator : DateTimeConversionTranslatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public SqlServerDateTimeConversionTranslator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        protected override SqlExpression ProcessTimeZoneId(SqlExpression timeZoneId, RelationalTypeMapping stringTypeMapping)
        {
            // SQL Server requires Windows timezone names
            // Convert IANA to Windows if it's a constant
            if (timeZoneId is SqlConstantExpression constantTz && constantTz.Value is string tzValue && TimeZoneInfo.TryConvertIanaIdToWindowsId(tzValue, out var windowsId))
            {
                return this.SqlExpressionFactory.Constant(windowsId, stringTypeMapping);
            }

            // For parameters or non-constant expressions, apply type mapping
            return timeZoneId.TypeMapping == null
                ? this.SqlExpressionFactory.ApplyTypeMapping(timeZoneId, stringTypeMapping)
                : timeZoneId;
        }

        /// <inheritdoc/>
        protected override SqlExpression GetFormatString(RelationalTypeMapping stringTypeMapping)
        {
            // SQL Server uses CONVERT with style 120 (hardcoded in QuerySqlGenerator)
            // This formatString is not used but kept for consistency
            return this.SqlExpressionFactory.Constant(string.Empty, stringTypeMapping);
        }
    }
}

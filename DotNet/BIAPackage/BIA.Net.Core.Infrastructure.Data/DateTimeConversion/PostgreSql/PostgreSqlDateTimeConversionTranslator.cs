// <copyright file="PostgreSqlDateTimeConversionTranslator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql
{
    using System;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// PostgreSQL-specific DateTime AT TIME ZONE translator.
    /// Uses IANA timezone IDs natively (no conversion needed).
    /// </summary>
    internal sealed class PostgreSqlDateTimeConversionTranslator : DateTimeConversionTranslatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public PostgreSqlDateTimeConversionTranslator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        protected override SqlExpression ProcessTimeZoneId(SqlExpression timeZoneId, RelationalTypeMapping stringTypeMapping)
        {
            // PostgreSQL uses IANA timezone IDs natively - no conversion needed
            // Just ensure type mapping is applied
            return timeZoneId.TypeMapping == null
                ? this.SqlExpressionFactory.ApplyTypeMapping(timeZoneId, stringTypeMapping)
                : timeZoneId;
        }

        /// <inheritdoc/>
        protected override SqlExpression GetFormatString(RelationalTypeMapping stringTypeMapping)
        {
            // PostgreSQL uses TO_CHAR with this format string
            return this.SqlExpressionFactory.Constant("YYYY-MM-DD HH24:MI:SS", stringTypeMapping);
        }
    }
}

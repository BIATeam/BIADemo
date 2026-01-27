// <copyright file="DateTimeConversionSqlNullabilityProcessor.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion
{
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

    /// <summary>
    /// Custom SQL nullability processor that knows how to handle DateTimeAtTimeZoneExpression.
    /// </summary>
    internal sealed class DateTimeConversionSqlNullabilityProcessor : SqlNullabilityProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionSqlNullabilityProcessor"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="parameters">The parameters.</param>
        public DateTimeConversionSqlNullabilityProcessor(
            RelationalParameterBasedSqlProcessorDependencies dependencies,
            RelationalParameterBasedSqlProcessorParameters parameters)
            : base(dependencies, parameters)
        {
        }

        /// <summary>
        /// Handles custom SQL expressions, including DateTimeFormatWithTimeZoneExpression.
        /// </summary>
        /// <param name="sqlExpression">The SQL expression to process.</param>
        /// <param name="allowOptimizedExpansion">Whether to allow optimized expansion.</param>
        /// <param name="nullable">Output parameter indicating if the result is nullable.</param>
        /// <returns>The processed SQL expression.</returns>
        protected override SqlExpression VisitCustomSqlExpression(
            SqlExpression sqlExpression,
            bool allowOptimizedExpansion,
            out bool nullable)
        {
            // Handle our DateTimeFormatWithTimeZoneExpression
            if (sqlExpression is DateTimeFormatWithTimeZoneExpression dateTimeFormat)
            {
                // Visit the datetime column
                var newDateTimeColumn = this.Visit(
                    dateTimeFormat.DateTimeColumn,
                    allowOptimizedExpansion,
                    out var columnNullable);

                // Visit the timezone ID parameter
                var newTimeZoneId = this.Visit(
                    dateTimeFormat.TimeZoneId,
                    allowOptimizedExpansion,
                    out var timezoneNullable);

                // Visit the format string constant
                var newFormatString = this.Visit(
                    dateTimeFormat.FormatString,
                    allowOptimizedExpansion,
                    out _);

                // The result is nullable if any input is nullable
                nullable = columnNullable || timezoneNullable;

                // Return updated expression if any child changed
                return dateTimeFormat.Update(newDateTimeColumn, newTimeZoneId, newFormatString);
            }

            // For any other custom expression, call base (which will throw)
            return base.VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable);
        }
    }
}

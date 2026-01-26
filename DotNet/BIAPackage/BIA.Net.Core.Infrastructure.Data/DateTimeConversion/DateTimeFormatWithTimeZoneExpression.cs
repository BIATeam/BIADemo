// <copyright file="DateTimeFormatWithTimeZoneExpression.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Custom SQL expression that generates the complete FORMAT([column] AT TIME ZONE 'UTC' AT TIME ZONE [timezone], 'format') SQL.
    /// This generates the entire SQL statement in one go to avoid composition issues.
    /// </summary>
    public class DateTimeFormatWithTimeZoneExpression : SqlExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatWithTimeZoneExpression"/> class.
        /// </summary>
        /// <param name="dateTimeColumn">The DateTime column expression.</param>
        /// <param name="timeZoneId">The target timezone ID expression.</param>
        /// <param name="formatString">The format string expression.</param>
        /// <param name="typeMapping">The type mapping for string result.</param>
        public DateTimeFormatWithTimeZoneExpression(
            SqlExpression dateTimeColumn,
            SqlExpression timeZoneId,
            SqlExpression formatString,
            RelationalTypeMapping typeMapping)
            : base(typeof(string), typeMapping)
        {
            this.DateTimeColumn = dateTimeColumn;
            this.TimeZoneId = timeZoneId;
            this.FormatString = formatString;
        }

        /// <summary>
        /// Gets the DateTime column expression.
        /// </summary>
        public SqlExpression DateTimeColumn { get; }

        /// <summary>
        /// Gets the target timezone ID expression.
        /// </summary>
        public SqlExpression TimeZoneId { get; }

        /// <summary>
        /// Gets the format string expression.
        /// </summary>
        public SqlExpression FormatString { get; }

        /// <inheritdoc/>
        public override SqlExpression Quote()
        {
            return new DateTimeFormatWithTimeZoneExpression(
                this.DateTimeColumn,
                this.TimeZoneId,
                this.FormatString,
                this.TypeMapping);
        }

        /// <summary>
        /// Creates a new expression with updated children.
        /// </summary>
        /// <param name="dateTimeColumn">The new DateTime column.</param>
        /// <param name="timeZoneId">The new timezone ID.</param>
        /// <param name="formatString">The new format string.</param>
        /// <returns>A new expression if children changed, otherwise this.</returns>
        public DateTimeFormatWithTimeZoneExpression Update(
            SqlExpression dateTimeColumn,
            SqlExpression timeZoneId,
            SqlExpression formatString)
        {
            return dateTimeColumn != this.DateTimeColumn || timeZoneId != this.TimeZoneId || formatString != this.FormatString
                ? new DateTimeFormatWithTimeZoneExpression(dateTimeColumn, timeZoneId, formatString, this.TypeMapping)
                : this;
        }

        /// <inheritdoc/>
        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            // Visit children to allow parameter binding and column references
            var newDateTimeColumn = (SqlExpression)visitor.Visit(this.DateTimeColumn);
            var newTimeZoneId = (SqlExpression)visitor.Visit(this.TimeZoneId);
            var newFormatString = (SqlExpression)visitor.Visit(this.FormatString);

            return this.Update(newDateTimeColumn, newTimeZoneId, newFormatString);
        }

        /// <inheritdoc/>
        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            // Generate the complete SQL: FORMAT([column] AT TIME ZONE 'UTC' AT TIME ZONE [timezone], [format])
            expressionPrinter.Append("FORMAT(");
            expressionPrinter.Visit(this.DateTimeColumn);
            expressionPrinter.Append(" AT TIME ZONE N'UTC' AT TIME ZONE ");
            expressionPrinter.Visit(this.TimeZoneId);
            expressionPrinter.Append(", ");
            expressionPrinter.Visit(this.FormatString);
            expressionPrinter.Append(")");
        }
    }
}

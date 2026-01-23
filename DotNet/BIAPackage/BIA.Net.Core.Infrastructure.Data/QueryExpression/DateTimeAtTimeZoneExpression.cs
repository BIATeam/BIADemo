// <copyright file="DateTimeAtTimeZoneExpression.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.QueryExpression
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Custom SQL expression that represents the AT TIME ZONE construct.
    /// Generates: [operand] AT TIME ZONE 'UTC' AT TIME ZONE [targetTimeZone].
    /// Works with both SQL Server and PostgreSQL.
    /// </summary>
    public class DateTimeAtTimeZoneExpression : SqlExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeAtTimeZoneExpression"/> class.
        /// </summary>
        /// <param name="operand">The DateTime operand.</param>
        /// <param name="utcTimeZone">The UTC time zone constant.</param>
        /// <param name="targetTimeZone">The target time zone expression.</param>
        /// <param name="type">The result type.</param>
        /// <param name="typeMapping">The type mapping.</param>
        public DateTimeAtTimeZoneExpression(
            SqlExpression operand,
            SqlExpression utcTimeZone,
            SqlExpression targetTimeZone,
            Type type,
            RelationalTypeMapping typeMapping)
            : base(type, typeMapping)
        {
            this.Operand = operand;
            this.UtcTimeZone = utcTimeZone;
            this.TargetTimeZone = targetTimeZone;
        }

        /// <summary>
        /// Gets the DateTime operand.
        /// </summary>
        public SqlExpression Operand { get; }

        /// <summary>
        /// Gets the UTC time zone constant.
        /// </summary>
        public SqlExpression UtcTimeZone { get; }

        /// <summary>
        /// Gets the target time zone expression.
        /// </summary>
        public SqlExpression TargetTimeZone { get; }

        /// <inheritdoc/>
        public override SqlExpression Quote()
        {
            return new DateTimeAtTimeZoneExpression(
                this.Operand,
                this.UtcTimeZone,
                this.TargetTimeZone,
                this.Type,
                this.TypeMapping);
        }

        /// <inheritdoc/>
        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var operand = (SqlExpression)visitor.Visit(this.Operand);
            var utcTimeZone = (SqlExpression)visitor.Visit(this.UtcTimeZone);
            var targetTimeZone = (SqlExpression)visitor.Visit(this.TargetTimeZone);

            return this.Update(operand, utcTimeZone, targetTimeZone);
        }

        /// <summary>
        /// Creates a new expression with the given children.
        /// </summary>
        /// <param name="operand">The new operand.</param>
        /// <param name="utcTimeZone">The new UTC time zone.</param>
        /// <param name="targetTimeZone">The new target time zone.</param>
        /// <returns>A new expression if children changed, otherwise this.</returns>
        public DateTimeAtTimeZoneExpression Update(SqlExpression operand, SqlExpression utcTimeZone, SqlExpression targetTimeZone)
        {
            return operand != this.Operand || utcTimeZone != this.UtcTimeZone || targetTimeZone != this.TargetTimeZone
                ? new DateTimeAtTimeZoneExpression(operand, utcTimeZone, targetTimeZone, this.Type, this.TypeMapping)
                : this;
        }

        /// <inheritdoc/>
        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            expressionPrinter.Append("(");
            expressionPrinter.Visit(this.Operand);
            expressionPrinter.Append(" AT TIME ZONE ");
            expressionPrinter.Visit(this.UtcTimeZone);
            expressionPrinter.Append(" AT TIME ZONE ");
            expressionPrinter.Visit(this.TargetTimeZone);
            expressionPrinter.Append(")");
        }
    }
}

// <copyright file="DateTimeConversionTranslator.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.QueryExpression
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
    /// Base class that handles all DateTime AT TIME ZONE translation logic.
    /// EF Core will inject ISqlExpressionFactory and IRelationalTypeMappingSource automatically.
    /// </summary>
    public abstract class DateTimeConversionTranslator : IMethodCallTranslatorPlugin, IMethodCallTranslator
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ISqlExpressionFactory sqlExpressionFactory;
        private readonly IRelationalTypeMappingSource typeMappingSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="sqlExpressionFactory">The SQL expression factory.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        protected DateTimeConversionTranslator(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.sqlExpressionFactory = serviceProvider.GetRequiredService<ISqlExpressionFactory>();
            this.typeMappingSource = serviceProvider.GetRequiredService<IRelationalTypeMappingSource>();
        }

        /// <summary>
        /// Gets the database provider type.
        /// </summary>
        protected abstract DbProvider DbProvider { get; }

        /// <inheritdoc/>
        public IEnumerable<IMethodCallTranslator> Translators => new[] { this };

        /// <inheritdoc/>
        public SqlExpression? Translate(
            SqlExpression? instance,
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
            // arguments[1] = timeZoneId string (expected to be Windows format from IClientTimeZoneContext.WindowsTimeZone.Id)
            var dateTimeColumn = arguments[0];
            var timeZoneId = arguments[1];

            var stringTypeMapping = this.typeMappingSource.FindMapping(typeof(string));

            // Apply type mapping to timezone if needed
            var processedTimeZone = timeZoneId.TypeMapping == null
                ? this.sqlExpressionFactory.ApplyTypeMapping(timeZoneId, stringTypeMapping)
                : timeZoneId;

            // Create the complete expression that generates: FORMAT([column] AT TIME ZONE 'UTC' AT TIME ZONE @timezone, 'format')
            // This avoids composition issues by generating all SQL in one Print() method
            var formatString = this.DbProvider == DbProvider.SqlServer
                ? "yyyy-MM-dd HH:mm:ss"
                : "YYYY-MM-DD HH24:MI:SS";

            return new DateTimeFormatWithTimeZoneExpression(
                dateTimeColumn,
                processedTimeZone,
                this.sqlExpressionFactory.Constant(formatString, stringTypeMapping),
                stringTypeMapping);
        }
    }

    /// <summary>
    /// SQL Server specific translator.
    /// </summary>
    public sealed class SqlServerDateTimeConversionTranslator : DateTimeConversionTranslator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="sqlExpressionFactory">The SQL expression factory.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        public SqlServerDateTimeConversionTranslator(
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        protected override DbProvider DbProvider => DbProvider.SqlServer;
    }

    /// <summary>
    /// PostgreSQL specific translator.
    /// </summary>
    public sealed class PostgreSqlDateTimeConversionTranslator : DateTimeConversionTranslator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="sqlExpressionFactory">The SQL expression factory.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        public PostgreSqlDateTimeConversionTranslator(
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        protected override DbProvider DbProvider => DbProvider.PostGreSql;
    }
}

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
    /// Uses lazy initialization of EF Core services.
    /// </summary>
    public abstract class DateTimeConversionTranslator : IMethodCallTranslatorPlugin, IMethodCallTranslator
    {
        private readonly IServiceProvider serviceProvider;
        private ISqlExpressionFactory? sqlExpressionFactory;
        private IRelationalTypeMappingSource? typeMappingSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionTranslator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve EF Core services lazily.</param>
        protected DateTimeConversionTranslator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the database provider type.
        /// </summary>
        protected abstract DbProvider DbProvider { get; }

        /// <summary>
        /// Gets the SQL expression factory, resolving it lazily on first use.
        /// </summary>
        private ISqlExpressionFactory SqlExpressionFactory 
            => this.sqlExpressionFactory ??= this.serviceProvider.GetRequiredService<ISqlExpressionFactory>();

        /// <summary>
        /// Gets the type mapping source, resolving it lazily on first use.
        /// </summary>
        private IRelationalTypeMappingSource TypeMappingSource 
            => this.typeMappingSource ??= this.serviceProvider.GetRequiredService<IRelationalTypeMappingSource>();

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
            if (method.DeclaringType?.FullName != "BIA.Net.Core.Infrastructure.Data.QueryExpression.DatabaseDateTimeExpressionConverter"
                || method.Name != "ConvertDateTimeToLocalString"
                || arguments.Count != 2)
            {
                return null;
            }

            // arguments[0] = DateTime column
            // arguments[1] = timeZoneId string
            var dateTimeColumn = arguments[0];
            var timeZoneId = arguments[1];

            // Create the AT TIME ZONE expression: column AT TIME ZONE 'UTC' AT TIME ZONE @timeZoneId
            var utcConstant = this.SqlExpressionFactory.Constant("UTC");
            var atTimeZoneExpression = new DateTimeAtTimeZoneExpression(
                dateTimeColumn,
                utcConstant,
                timeZoneId,
                typeof(DateTimeOffset),
                this.TypeMappingSource.FindMapping(typeof(DateTimeOffset)));

            // Wrap with FORMAT (SQL Server) or TO_CHAR (PostgreSQL)
            return this.DbProvider == DbProvider.SqlServer
                ? this.SqlExpressionFactory.Function(
                    "FORMAT",
                    new[] { atTimeZoneExpression, this.SqlExpressionFactory.Constant("yyyy-MM-dd HH:mm:ss") },
                    nullable: true,
                    argumentsPropagateNullability: new[] { true, false },
                    typeof(string),
                    this.TypeMappingSource.FindMapping(typeof(string)))
                : this.SqlExpressionFactory.Function(
                    "TO_CHAR",
                    new[] { atTimeZoneExpression, this.SqlExpressionFactory.Constant("YYYY-MM-DD HH24:MI:SS") },
                    nullable: true,
                    argumentsPropagateNullability: new[] { true, false },
                    typeof(string),
                    this.TypeMappingSource.FindMapping(typeof(string)));
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
        /// <param name="serviceProvider">The service provider.</param>
        public SqlServerDateTimeConversionTranslator(IServiceProvider serviceProvider)
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
        /// <param name="serviceProvider">The service provider.</param>
        public PostgreSqlDateTimeConversionTranslator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        protected override DbProvider DbProvider => DbProvider.PostGreSql;
    }
}

// <copyright file="DateTimeConversionTranslatorBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Base class for database-specific DateTime AT TIME ZONE translation logic.
    /// Implements the common translation logic with provider-specific customization points.
    /// </summary>
    internal abstract class DateTimeConversionTranslatorBase : IMethodCallTranslatorPlugin, IMethodCallTranslator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionTranslatorBase"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected DateTimeConversionTranslatorBase(IServiceProvider serviceProvider)
        {
            this.SqlExpressionFactory = serviceProvider.GetRequiredService<ISqlExpressionFactory>();
            this.TypeMappingSource = serviceProvider.GetRequiredService<IRelationalTypeMappingSource>();
        }

        /// <inheritdoc/>
        public IEnumerable<IMethodCallTranslator> Translators => new[] { this };

        /// <summary>
        /// Gets the SQL expression factory.
        /// </summary>
        protected ISqlExpressionFactory SqlExpressionFactory { get; }

        /// <summary>
        /// Gets the type mapping source.
        /// </summary>
        protected IRelationalTypeMappingSource TypeMappingSource { get; }

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

            var stringTypeMapping = this.TypeMappingSource.FindMapping(typeof(string));

            // Process timezone (provider-specific)
            var processedTimeZone = this.ProcessTimeZoneId(timeZoneId, stringTypeMapping);

            // Get format string (provider-specific)
            var formatString = this.GetFormatString(stringTypeMapping);

            return new DateTimeFormatWithTimeZoneExpression(
                dateTimeColumn,
                processedTimeZone,
                formatString,
                stringTypeMapping);
        }

        /// <summary>
        /// Process the timezone ID expression for the specific database provider.
        /// </summary>
        /// <param name="timeZoneId">The timezone ID expression.</param>
        /// <param name="stringTypeMapping">The string type mapping.</param>
        /// <returns>The processed timezone expression.</returns>
        protected abstract SqlExpression ProcessTimeZoneId(SqlExpression timeZoneId, RelationalTypeMapping stringTypeMapping);

        /// <summary>
        /// Get the format string expression for the specific database provider.
        /// </summary>
        /// <param name="stringTypeMapping">The string type mapping.</param>
        /// <returns>The format string expression.</returns>
        protected abstract SqlExpression GetFormatString(RelationalTypeMapping stringTypeMapping);
    }
}

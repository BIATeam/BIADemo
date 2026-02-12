// <copyright file="SqlServerDateTimeConversionParameterBasedSqlProcessor.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.SqlServer
{
    using System.Linq.Expressions;
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

    /// <summary>
    /// Custom SQL parameter processor for SQL Server that uses our custom nullability processor
    /// and converts IANA timezone parameters to Windows timezones.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    internal sealed class SqlServerDateTimeConversionParameterBasedSqlProcessor : SqlServerParameterBasedSqlProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionParameterBasedSqlProcessor"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sqlServerOptions">The SQL Server options.</param>
        public SqlServerDateTimeConversionParameterBasedSqlProcessor(
            RelationalParameterBasedSqlProcessorDependencies dependencies,
            RelationalParameterBasedSqlProcessorParameters parameters,
            Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.ISqlServerSingletonOptions sqlServerOptions)
            : base(dependencies, parameters, sqlServerOptions)
        {
        }

        /// <summary>
        /// Processes SQL nullability using our custom processor.
        /// </summary>
        /// <param name="selectExpression">The select expression.</param>
        /// <param name="Decorator">The parameters decorator.</param>
        /// <returns>The processed expression.</returns>
        protected override Expression ProcessSqlNullability(
            Expression selectExpression,
            ParametersCacheDecorator Decorator)
        {
            // Use our custom nullability processor
            var processor = new DateTimeConversionSqlNullabilityProcessor(
                this.Dependencies,
                this.Parameters);

            return processor.Process(selectExpression, Decorator);
        }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}

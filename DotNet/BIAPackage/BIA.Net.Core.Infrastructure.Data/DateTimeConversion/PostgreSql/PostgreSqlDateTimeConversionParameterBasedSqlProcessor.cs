// <copyright file="PostgreSqlDateTimeConversionParameterBasedSqlProcessor.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Query;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;

    /// <summary>
    /// Custom SQL parameter processor for PostgreSQL that uses our custom nullability processor.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    internal sealed class PostgreSqlDateTimeConversionParameterBasedSqlProcessor : NpgsqlParameterBasedSqlProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionParameterBasedSqlProcessor"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="parameters">The parameters.</param>
        public PostgreSqlDateTimeConversionParameterBasedSqlProcessor(
            RelationalParameterBasedSqlProcessorDependencies dependencies,
            RelationalParameterBasedSqlProcessorParameters parameters)
            : base(dependencies, parameters)
        {
        }

        /// <summary>
        /// Processes SQL nullability using our custom processor.
        /// </summary>
        /// <param name="selectExpression">The select expression.</param>
        /// <param name="parametersDecorator">The parameters decorator.</param>
        /// <returns>The processed expression.</returns>
        protected override Expression ProcessSqlNullability(
            Expression selectExpression,
            ParametersCacheDecorator parametersDecorator)
        {
            // Use our custom nullability processor
            var processor = new DateTimeConversionSqlNullabilityProcessor(
                this.Dependencies,
                this.Parameters);

            return processor.Process(selectExpression, parametersDecorator);
        }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}

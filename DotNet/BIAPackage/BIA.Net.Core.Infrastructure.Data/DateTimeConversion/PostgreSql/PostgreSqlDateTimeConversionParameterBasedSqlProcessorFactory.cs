// <copyright file="PostgreSqlDateTimeConversionParameterBasedSqlProcessorFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql
{
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql;
    using Microsoft.EntityFrameworkCore.Query;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

    /// <summary>
    /// Factory for creating PostgreSQL parameter-based SQL processors with custom nullability handling.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class PostgreSqlDateTimeConversionParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly RelationalParameterBasedSqlProcessorDependencies dependencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionParameterBasedSqlProcessorFactory"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="npgsqlOptions">The PostgreSQL options.</param>
        public PostgreSqlDateTimeConversionParameterBasedSqlProcessorFactory(
            RelationalParameterBasedSqlProcessorDependencies dependencies,
            INpgsqlSingletonOptions npgsqlOptions)
        {
            this.dependencies = dependencies;
        }

        /// <inheritdoc/>
        public RelationalParameterBasedSqlProcessor Create(RelationalParameterBasedSqlProcessorParameters parameters)
        {
            return new PostgreSqlDateTimeConversionParameterBasedSqlProcessor(
                this.dependencies,
                parameters);
        }
#pragma warning restore EF1001 // Internal EF Core API usage.
    }
}

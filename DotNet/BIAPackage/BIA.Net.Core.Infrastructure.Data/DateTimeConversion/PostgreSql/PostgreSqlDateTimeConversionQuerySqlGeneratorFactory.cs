// <copyright file="PostgreSqlDateTimeConversionQuerySqlGeneratorFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql
{
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Storage;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

    /// <summary>
    /// Factory for creating PostgreSQL query SQL generators with custom DateTime conversion support.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class PostgreSqlDateTimeConversionQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies dependencies;
        private readonly IRelationalTypeMappingSource typeMappingSource;
        private readonly INpgsqlSingletonOptions npgsqlOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionQuerySqlGeneratorFactory"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        /// <param name="npgsqlOptions">The PostgreSQL options.</param>
        public PostgreSqlDateTimeConversionQuerySqlGeneratorFactory(
            QuerySqlGeneratorDependencies dependencies,
            IRelationalTypeMappingSource typeMappingSource,
            INpgsqlSingletonOptions npgsqlOptions)
        {
            this.dependencies = dependencies;
            this.typeMappingSource = typeMappingSource;
            this.npgsqlOptions = npgsqlOptions;
        }

        /// <inheritdoc/>
        public QuerySqlGenerator Create()
        {
            return new PostgreSqlDateTimeConversionQuerySqlGenerator(
                this.dependencies,
                this.typeMappingSource,
                this.npgsqlOptions.ReverseNullOrderingEnabled,
                this.npgsqlOptions.PostgresVersion);
        }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}

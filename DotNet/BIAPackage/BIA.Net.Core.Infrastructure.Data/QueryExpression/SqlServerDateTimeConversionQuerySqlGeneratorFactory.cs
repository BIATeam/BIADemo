// <copyright file="SqlServerDateTimeConversionQuerySqlGeneratorFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.QueryExpression
{
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Factory for creating SQL Server query SQL generators with custom DateTime conversion support.
    /// </summary>
    public class SqlServerDateTimeConversionQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies dependencies;
        private readonly IRelationalTypeMappingSource typeMappingSource;
        private readonly ISqlServerSingletonOptions sqlServerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionQuerySqlGeneratorFactory"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        /// <param name="sqlServerOptions">The SQL Server options.</param>
        public SqlServerDateTimeConversionQuerySqlGeneratorFactory(
            QuerySqlGeneratorDependencies dependencies,
            IRelationalTypeMappingSource typeMappingSource,
            ISqlServerSingletonOptions sqlServerOptions)
        {
            this.dependencies = dependencies;
            this.typeMappingSource = typeMappingSource;
            this.sqlServerOptions = sqlServerOptions;
        }

        /// <inheritdoc/>
        public QuerySqlGenerator Create()
        {
            return new SqlServerDateTimeConversionQuerySqlGenerator(
                this.dependencies,
                this.typeMappingSource,
                this.sqlServerOptions);
        }
    }
}

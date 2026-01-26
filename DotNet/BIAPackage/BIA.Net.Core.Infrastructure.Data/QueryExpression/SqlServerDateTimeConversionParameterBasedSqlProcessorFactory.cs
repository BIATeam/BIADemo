// <copyright file="SqlServerDateTimeConversionParameterBasedSqlProcessorFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.QueryExpression
{
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

    /// <summary>
    /// Factory for creating SQL Server parameter-based SQL processors with custom nullability handling.
    /// </summary>
    public class SqlServerDateTimeConversionParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly RelationalParameterBasedSqlProcessorDependencies dependencies;
        private readonly ISqlServerSingletonOptions sqlServerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionParameterBasedSqlProcessorFactory"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="sqlServerOptions">The SQL Server options.</param>
        public SqlServerDateTimeConversionParameterBasedSqlProcessorFactory(
            RelationalParameterBasedSqlProcessorDependencies dependencies,
            ISqlServerSingletonOptions sqlServerOptions)
        {
            this.dependencies = dependencies;
            this.sqlServerOptions = sqlServerOptions;
        }

        /// <inheritdoc/>
        public RelationalParameterBasedSqlProcessor Create(RelationalParameterBasedSqlProcessorParameters parameters)
        {
            return new SqlServerDateTimeConversionParameterBasedSqlProcessor(
                this.dependencies,
                parameters,
                this.sqlServerOptions);
        }
    }
}

// <copyright file="BiaSqlServerHistoryRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories
{
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// History Repository for BIA SQL Server.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class BiaSqlServerHistoryRepository : SqlServerHistoryRepository
    {
        private readonly BiaHistoryRepositoryOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaSqlServerHistoryRepository"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        public BiaSqlServerHistoryRepository(HistoryRepositoryDependencies dependencies)
            : base(dependencies)
#pragma warning restore EF1001 // Internal EF Core API usage.
        {
            this.options = dependencies.CurrentContext.Context.GetService<IOptions<BiaHistoryRepositoryOptions>>().Value;
        }

        /// <inheritdoc/>
        public override string GetCreateIfNotExistsScript()
        {
            var table = this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema);
            var migrationId = this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName);
            var productVersion = this.SqlGenerationHelper.DelimitIdentifier(this.ProductVersionColumnName);

            return $@"
IF OBJECT_ID(N'{table}', N'U') IS NULL
BEGIN
    CREATE TABLE {table} (
        {migrationId} nvarchar(150) NOT NULL CONSTRAINT PK_{this.TableName} PRIMARY KEY,
        {productVersion} nvarchar(32) NOT NULL,
        [MigratedAt]  datetime2 NULL,
        [AppVersion] nvarchar(64) NULL
    );
END";
        }

        /// <inheritdoc/>
        public override string GetInsertScript(HistoryRow row)
        {
            var mappingString = this.Dependencies.TypeMappingSource.FindMapping(typeof(string))!;
            var table = this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema);
            var migrationId = this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName);
            var productVersion = this.SqlGenerationHelper.DelimitIdentifier(this.ProductVersionColumnName);

            var appVersion = !string.IsNullOrWhiteSpace(this.options.AppVersion)
                ? mappingString.GenerateSqlLiteral(this.options.AppVersion!)
                : "NULL";

            return $@"
INSERT INTO {table} ({migrationId}, {productVersion}, [MigratedAt], [AppVersion])
VALUES ({mappingString.GenerateSqlLiteral(row.MigrationId)},
        {mappingString.GenerateSqlLiteral(row.ProductVersion)},
        sysutcdatetime(),
        {appVersion});";
        }
    }
}

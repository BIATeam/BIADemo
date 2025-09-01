namespace BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
    using Microsoft.Extensions.Options;

    public class BiaSqlServerHistoryRepository : SqlServerHistoryRepository
    {
        private readonly BiaHistoryRepositoryOptions options;

        public BiaSqlServerHistoryRepository(HistoryRepositoryDependencies dependencies)
#pragma warning disable EF1001 // Internal EF Core API usage.
            : base(dependencies)
#pragma warning restore EF1001 // Internal EF Core API usage.
        {
            this.options = dependencies.CurrentContext.Context.GetService<IOptions<BiaHistoryRepositoryOptions>>()?.Value ?? throw new Common.Exceptions.BadBiaFrameworkUsageException("BiaHistoryRepositoryOptions not configured");
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

            var migratedAt = this.options.StampMigratedAt ? "sysutcdatetime()" : "NULL";
            var appVersion = (this.options.StampAppVersion && !string.IsNullOrWhiteSpace(this.options.AppVersion))
                ? mappingString.GenerateSqlLiteral(this.options.AppVersion!)
                : "NULL";

            return $@"
                INSERT INTO {table} ({migrationId}, {productVersion}, [MigratedAt], [AppVersion])
                VALUES ({mappingString.GenerateSqlLiteral(row.MigrationId)},
                        {mappingString.GenerateSqlLiteral(row.ProductVersion)},
                        {migratedAt},
                        {appVersion});";
        }
    }
}

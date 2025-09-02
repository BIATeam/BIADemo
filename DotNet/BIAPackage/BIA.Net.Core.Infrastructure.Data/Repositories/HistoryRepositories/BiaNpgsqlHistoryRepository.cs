// <copyright file="BiaNpgsqlHistoryRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories
{
    using System;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Options;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

    /// <summary>
    /// History Repository for BIA PostgreSQL.
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class BiaNpgsqlHistoryRepository : NpgsqlHistoryRepository
    {
        private readonly BiaHistoryRepositoryOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaNpgsqlHistoryRepository"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        public BiaNpgsqlHistoryRepository(HistoryRepositoryDependencies dependencies)
            : base(dependencies)
#pragma warning restore EF1001 // Internal EF Core API usage.
        {
            this.options = dependencies.CurrentContext.Context
                .GetService<IOptions<BiaHistoryRepositoryOptions>>().Value;
        }

        /// <inheritdoc/>
        public override string GetCreateIfNotExistsScript()
        {
            var table = this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema);
            var migrationId = this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName);
            var productVersion = this.SqlGenerationHelper.DelimitIdentifier(this.ProductVersionColumnName);
            var migratedAt = this.SqlGenerationHelper.DelimitIdentifier("MigratedAt");
            var appVersion = this.SqlGenerationHelper.DelimitIdentifier("AppVersion");

            return $@"
CREATE TABLE IF NOT EXISTS {table} (
    {migrationId}    character varying(150) NOT NULL PRIMARY KEY,
    {productVersion} character varying(32)  NOT NULL,
    {migratedAt}     timestamp with time zone NULL,
    {appVersion}     character varying(64)  NULL
);";
        }

        /// <inheritdoc/>
        public override string GetInsertScript(HistoryRow row)
        {
            var stringMapping = this.Dependencies.TypeMappingSource.FindMapping(typeof(string))!;
            var table = this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema);
            var migrationId = this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName);
            var productVersion = this.SqlGenerationHelper.DelimitIdentifier(this.ProductVersionColumnName);
            var migratedAt = this.SqlGenerationHelper.DelimitIdentifier("MigratedAt");
            var appVersionCol = this.SqlGenerationHelper.DelimitIdentifier("AppVersion");

            var appVersionVal = !string.IsNullOrWhiteSpace(this.options.AppVersion)
                ? stringMapping.GenerateSqlLiteral(this.options.AppVersion!)
                : "NULL";

            return $@"
INSERT INTO {table} ({migrationId}, {productVersion}, {migratedAt}, {appVersionCol})
VALUES ({stringMapping.GenerateSqlLiteral(row.MigrationId)},
        {stringMapping.GenerateSqlLiteral(row.ProductVersion)},
        now(),
        {appVersionVal});";
        }

        /// <inheritdoc/>
        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);

            history.Property<string>("AppVersion").HasMaxLength(64);
            history.Property<DateTime?>("MigratedAt");
        }
    }
}

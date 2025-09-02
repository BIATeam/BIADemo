// <copyright file="BiaNpgsqlHistoryRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories
{
    using System;
    using Microsoft.EntityFrameworkCore;
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
        public override string GetInsertScript(HistoryRow row)
        {
            var mappingString = this.Dependencies.TypeMappingSource.FindMapping(typeof(string));
            var table = this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema);
            var migrationIdColumn = this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName);
            var migrationId = mappingString.GenerateSqlLiteral(row.MigrationId);
            var appColumn = this.SqlGenerationHelper.DelimitIdentifier("AppVersion");
            var appVersion = !string.IsNullOrWhiteSpace(this.options.AppVersion)
                ? mappingString.GenerateSqlLiteral(this.options.AppVersion)
                : "NULL";

            var updateScript = $@"
UPDATE {table}
SET {appColumn} = {appVersion}
WHERE {migrationIdColumn} = {migrationId};
";

            return base.GetInsertScript(row) + updateScript;
        }

        /// <inheritdoc/>
        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);

            history.Property<string>("AppVersion").HasMaxLength(64);
            history.Property<DateTimeOffset?>("MigratedAt").HasDefaultValueSql("now()");
        }
    }
}

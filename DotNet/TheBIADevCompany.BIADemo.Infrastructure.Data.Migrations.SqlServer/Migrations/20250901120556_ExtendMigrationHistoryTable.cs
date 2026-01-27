using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBIADevCompany.BIADemo.Crosscutting.Common;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ExtendMigrationHistoryTable : Migration
    {
        private const string HistoryTable = "__EFMigrationsHistory";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.IsSqlServer())
            {
                migrationBuilder.Sql($@"
IF COL_LENGTH(N'{HistoryTable}', N'AppVersion') IS NULL
BEGIN
    ALTER TABLE [{HistoryTable}] ADD [AppVersion] nvarchar(64) NULL;
END;

IF COL_LENGTH(N'{HistoryTable}', N'MigratedAt') IS NULL
BEGIN
    ALTER TABLE [{HistoryTable}] ADD [MigratedAt] datetime2 NULL;
END;

IF COL_LENGTH(N'{HistoryTable}', N'MigratedAt') IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE OBJECT_NAME(dc.parent_object_id) = '{HistoryTable}' AND c.name = 'MigratedAt'
)
BEGIN
    ALTER TABLE [{HistoryTable}]
        ADD CONSTRAINT [DF_{HistoryTable}_MigratedAt] DEFAULT (sysutcdatetime()) FOR [MigratedAt];
END;
");
            }
            else if (migrationBuilder.IsNpgsql())
            {
                migrationBuilder.Sql($@"
ALTER TABLE ""{HistoryTable}""
    ADD COLUMN IF NOT EXISTS ""AppVersion"" character varying(64) NULL;

ALTER TABLE ""{HistoryTable}""
    ADD COLUMN IF NOT EXISTS ""MigratedAt"" timestamp with time zone NULL;

ALTER TABLE ""{HistoryTable}""
    ALTER COLUMN ""MigratedAt"" SET DEFAULT now();
");
            }
            else
            {
                throw new NotSupportedException($"Not supported provider : {migrationBuilder.ActiveProvider}");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.IsSqlServer())
            {
                migrationBuilder.Sql($@"
DECLARE @df sysname;
SELECT @df = dc.name
FROM sys.default_constraints dc
JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
WHERE OBJECT_NAME(dc.parent_object_id) = '{HistoryTable}' AND c.name = 'MigratedAt';
IF @df IS NOT NULL
    EXEC('ALTER TABLE [{HistoryTable}] DROP CONSTRAINT [' + @df + ']');

IF COL_LENGTH(N'{HistoryTable}', N'MigratedAt') IS NOT NULL
    ALTER TABLE [{HistoryTable}] DROP COLUMN [MigratedAt];

IF COL_LENGTH(N'{HistoryTable}', N'AppVersion') IS NOT NULL
    ALTER TABLE [{HistoryTable}] DROP COLUMN [AppVersion];
");
            }
            else if (migrationBuilder.IsNpgsql())
            {
                migrationBuilder.Sql($@"
ALTER TABLE ""{HistoryTable}""
    ALTER COLUMN ""MigratedAt"" DROP DEFAULT;

ALTER TABLE ""{HistoryTable}""
    DROP COLUMN IF EXISTS ""MigratedAt"";

ALTER TABLE ""{HistoryTable}""
    DROP COLUMN IF EXISTS ""AppVersion"";");
            }
            else
            {
                throw new NotSupportedException($"Not supported provider : {migrationBuilder.ActiveProvider}");
            }
        }
    }
}

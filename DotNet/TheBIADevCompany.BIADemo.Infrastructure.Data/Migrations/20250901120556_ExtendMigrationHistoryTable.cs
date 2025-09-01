using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendMigrationHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            IF COL_LENGTH(N'__EFMigrationsHistory', N'MigratedAt') IS NULL
            BEGIN
                ALTER TABLE [__EFMigrationsHistory] ADD [MigratedAt] datetime2 NULL;
            END;

            IF COL_LENGTH(N'__EFMigrationsHistory', N'AppVersion') IS NULL
            BEGIN
                ALTER TABLE [__EFMigrationsHistory] ADD [AppVersion] nvarchar(64) NULL;
            END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            IF COL_LENGTH(N'__EFMigrationsHistory', N'MigratedAt') IS NOT NULL
            BEGIN
                ALTER TABLE [__EFMigrationsHistory] DROP COLUMN [MigratedAt];
            END;

            IF COL_LENGTH(N'__EFMigrationsHistory', N'AppVersion') IS NOT NULL
            BEGIN
                ALTER TABLE [__EFMigrationsHistory] DROP COLUMN [AppVersion];
            END;
            ");
        }
    }
}

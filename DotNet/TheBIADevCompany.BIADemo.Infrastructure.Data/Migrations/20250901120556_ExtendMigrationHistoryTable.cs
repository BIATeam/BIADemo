using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBIADevCompany.BIADemo.Crosscutting.Common;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendMigrationHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.IsSqlServer())
            {
                migrationBuilder.AddColumn<string>(
                    name: "AppVersion",
                    table: "__EFMigrationsHistory",
                    type: "nvarchar(64)",
                    nullable: true);

                migrationBuilder.AddColumn<DateTime?>(
                    name: "MigratedAt",
                    table: "__EFMigrationsHistory",
                    type: "datetime2",
                    nullable: true,
                    defaultValueSql: "sysutcdatetime()");
            }

            if (migrationBuilder.IsNpgsql())
            {
                migrationBuilder.AddColumn<string>(
                    name: "AppVersion",
                    table: "__EFMigrationsHistory",
                    type: "varchar(64)",
                    nullable: true);

                migrationBuilder.AddColumn<DateTimeOffset?>(
                    name: "MigratedAt",
                    table: "__EFMigrationsHistory",
                    type: "timestamp with time zone",
                    nullable: true);

                migrationBuilder.Sql(@"
ALTER TABLE public.""__EFMigrationsHistory""
ALTER COLUMN ""MigratedAt"" SET DEFAULT now();");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.IsSqlServer() || migrationBuilder.IsNpgsql())
            {
                migrationBuilder.DropColumn("MigratedAt", "__EFMigrationsHistory");
                migrationBuilder.DropColumn("AppVersion", "__EFMigrationsHistory");
            }
        }
    }
}

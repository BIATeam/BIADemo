using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAuditEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ModifyAuditEntity_Up(migrationBuilder, "UsersAudit");
            ModifyAuditEntity_Up(migrationBuilder, "AirportsAudit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ModifyAuditEntity_Down(migrationBuilder, "UsersAudit");
            ModifyAuditEntity_Down(migrationBuilder, "AirportsAudit");
        }

        private static void ModifyAuditEntity_Up(MigrationBuilder migrationBuilder, string table)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: table,
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql($@"
                UPDATE {table}
                SET EntityId = CAST(Id AS nvarchar(max))
            ");

            migrationBuilder.DropColumn(
                name: "Id",
                table: table);

            migrationBuilder.DropPrimaryKey(
                name: $"PK_{table}",
                table: table);

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: table,
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: $"PK_{table}",
                table: table,
                column: "Id");
        }

        private static void ModifyAuditEntity_Down(MigrationBuilder migrationBuilder, string table)
        {
            migrationBuilder.DropPrimaryKey(
                name: $"PK_{table}",
                table: table);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: table,
                newName: "AuditId");

            migrationBuilder.AddPrimaryKey(
                name: $"PK_{table}",
                table: table,
                column: "AuditId");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: table,
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql($@"
                UPDATE {table}
                SET Id = TRY_CAST(EntityId AS int)
            ");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: table);
        }
    }
}

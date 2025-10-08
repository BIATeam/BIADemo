using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateAuditEntity_Up(migrationBuilder, "UsersAudit");

            migrationBuilder.DropTable(
                name: "AirportsAudit");

            migrationBuilder.CreateTable(
                name: "EngineAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedEntities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAirportAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirportId = table.Column<int>(type: "int", nullable: false),
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    AirportName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaneName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedEntities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirportAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentAirportName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedEntities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAudit", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            UpdateAuditEntity_Down(migrationBuilder, "UsersAudit");

            migrationBuilder.DropTable(
                name: "EngineAudit");

            migrationBuilder.DropTable(
                name: "PlaneAirportAudit");

            migrationBuilder.DropTable(
                name: "PlaneAudit");

            migrationBuilder.CreateTable(
                name: "AirportsAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportsAudit", x => x.AuditId);
                });
        }

        private static void UpdateAuditEntity_Up(MigrationBuilder migrationBuilder, string table)
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

            migrationBuilder.AddColumn<string>(
                name: "LinkedEntities",
                table: table,
                type: "nvarchar(max)",
                nullable: true);
        }

        private static void UpdateAuditEntity_Down(MigrationBuilder migrationBuilder, string table)
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

            migrationBuilder.DropColumn(
                name: "LinkedEntities",
                table: table);
        }
    }
}

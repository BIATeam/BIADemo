using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirportsAudit");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "UsersAudit");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AuditLogs",
                newName: "AuditId");

            migrationBuilder.AlterColumn<string>(
                name: "AuditUserLogin",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditChanges",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditAction",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryKey",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditUserLogin",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditChanges",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditAction",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EngineAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAirportAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirportId = table.Column<int>(type: "int", nullable: false),
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    AirportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirportAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAudit", x => x.AuditId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EngineAudit");

            migrationBuilder.DropTable(
                name: "PlaneAirportAudit");

            migrationBuilder.DropTable(
                name: "PlaneAudit");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "AuditLogs",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "AuditUserLogin",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuditChanges",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuditAction",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "UsersAudit",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryKey",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuditUserLogin",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuditChanges",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuditAction",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AuditLogs",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

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
    }
}

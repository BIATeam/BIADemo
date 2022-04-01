using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class Audit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirportsAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportsAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "--"),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAudit", x => x.AuditId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirportsAudit");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "UsersAudit");
        }
    }
}

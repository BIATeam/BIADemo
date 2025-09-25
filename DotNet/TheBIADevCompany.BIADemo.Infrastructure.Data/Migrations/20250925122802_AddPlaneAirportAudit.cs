using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaneAirportAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaneAirportAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirportId = table.Column<int>(type: "int", nullable: false),
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentEntityId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirportAudit", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaneAirportAudit");
        }
    }
}

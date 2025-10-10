using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLinkedEntitiesAuditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedEntities",
                table: "UsersAudit");

            migrationBuilder.DropColumn(
                name: "CurrentAirportName",
                table: "PlaneAudit");

            migrationBuilder.DropColumn(
                name: "LinkedEntities",
                table: "PlaneAudit");

            migrationBuilder.DropColumn(
                name: "LinkedEntities",
                table: "PlaneAirportAudit");

            migrationBuilder.DropColumn(
                name: "LinkedEntities",
                table: "EngineAudit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedEntities",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentAirportName",
                table: "PlaneAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedEntities",
                table: "PlaneAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedEntities",
                table: "PlaneAirportAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedEntities",
                table: "EngineAudit",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

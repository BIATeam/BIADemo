using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AirportName",
                table: "PlaneAirportAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaneName",
                table: "PlaneAirportAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "EngineAudit",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirportName",
                table: "PlaneAirportAudit");

            migrationBuilder.DropColumn(
                name: "PlaneName",
                table: "PlaneAirportAudit");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "EngineAudit");
        }
    }
}

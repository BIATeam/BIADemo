using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaneName",
                table: "PlaneAirportAudit");

            migrationBuilder.AddColumn<string>(
                name: "CurrentAirportName",
                table: "PlaneAudit",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAirportName",
                table: "PlaneAudit");

            migrationBuilder.AddColumn<string>(
                name: "PlaneName",
                table: "PlaneAirportAudit",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

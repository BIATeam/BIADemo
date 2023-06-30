using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class PlaneAirport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaneAirport",
                table: "PlaneAirport");

            migrationBuilder.DropIndex(
                name: "IX_PlaneAirport_AirportId",
                table: "PlaneAirport");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaneAirport",
                table: "PlaneAirport",
                columns: new[] { "AirportId", "PlaneId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlaneAirport_PlaneId",
                table: "PlaneAirport",
                column: "PlaneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaneAirport",
                table: "PlaneAirport");

            migrationBuilder.DropIndex(
                name: "IX_PlaneAirport_PlaneId",
                table: "PlaneAirport");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaneAirport",
                table: "PlaneAirport",
                columns: new[] { "PlaneId", "AirportId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlaneAirport_AirportId",
                table: "PlaneAirport",
                column: "AirportId");
        }
    }
}

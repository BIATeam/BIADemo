using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class AddSiteOnPlanesAddAirportsAndPlanesTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaneTypeId",
                table: "Planes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Planes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    City = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Title = table.Column<string>(maxLength: 64, nullable: false),
                    CertificationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAirport",
                columns: table => new
                {
                    PlaneId = table.Column<int>(nullable: false),
                    AirportId = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirport", x => new { x.PlaneId, x.AirportId });
                    table.ForeignKey(
                        name: "FK_PlaneAirport_Airports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaneAirport_Planes_PlaneId",
                        column: x => x.PlaneId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Planes_PlaneTypeId",
                table: "Planes",
                column: "PlaneTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_SiteId",
                table: "Planes",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaneAirport_AirportId",
                table: "PlaneAirport",
                column: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_PlanesTypes_PlaneTypeId",
                table: "Planes",
                column: "PlaneTypeId",
                principalTable: "PlanesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Sites_SiteId",
                table: "Planes",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_PlanesTypes_PlaneTypeId",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Sites_SiteId",
                table: "Planes");

            migrationBuilder.DropTable(
                name: "PlaneAirport");

            migrationBuilder.DropTable(
                name: "PlanesTypes");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropIndex(
                name: "IX_Planes_PlaneTypeId",
                table: "Planes");

            migrationBuilder.DropIndex(
                name: "IX_Planes_SiteId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "PlaneTypeId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Planes");
        }
    }
}

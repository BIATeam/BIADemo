using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    AircraftMaintenanceCompanyId = table.Column<int>(type: "int", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFixed = table.Column<bool>(type: "bit", nullable: false),
                    FixedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                        column: x => x.AircraftMaintenanceCompanyId,
                        principalTable: "AircraftMaintenanceCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceContract_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceContractPlane",
                columns: table => new
                {
                    MaintenanceContractId = table.Column<int>(type: "int", nullable: false),
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceContractPlane", x => new { x.MaintenanceContractId, x.PlaneId });
                    table.ForeignKey(
                        name: "FK_MaintenanceContractPlane_MaintenanceContract_MaintenanceContractId",
                        column: x => x.MaintenanceContractId,
                        principalTable: "MaintenanceContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceContractPlane_Planes_PlaneId",
                        column: x => x.PlaneId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceContract_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract",
                column: "AircraftMaintenanceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceContract_SiteId",
                table: "MaintenanceContract",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceContractPlane_PlaneId",
                table: "MaintenanceContractPlane",
                column: "PlaneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceContractPlane");

            migrationBuilder.DropTable(
                name: "MaintenanceContract");
        }
    }
}

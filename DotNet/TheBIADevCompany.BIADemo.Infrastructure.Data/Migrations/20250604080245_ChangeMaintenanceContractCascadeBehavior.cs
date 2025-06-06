using Microsoft.EntityFrameworkCore.Migrations;
using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMaintenanceContractCascadeBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM[dbo].[MaintenanceContract] WHERE SiteId is null or AircraftMaintenanceCompanyId is null");
            
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_Sites_SiteId",
                table: "MaintenanceContract");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract",
                column: "AircraftMaintenanceCompanyId",
                principalTable: "AircraftMaintenanceCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceContract_Sites_SiteId",
                table: "MaintenanceContract",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_Sites_SiteId",
                table: "MaintenanceContract");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract",
                column: "AircraftMaintenanceCompanyId",
                principalTable: "AircraftMaintenanceCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceContract_Sites_SiteId",
                table: "MaintenanceContract",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

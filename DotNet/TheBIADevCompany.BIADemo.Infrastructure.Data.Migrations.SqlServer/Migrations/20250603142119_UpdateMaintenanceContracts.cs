using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaintenanceContracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_Sites_SiteId",
                table: "MaintenanceContract");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "MaintenanceContract",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AircraftMaintenanceCompanyId",
                table: "MaintenanceContract",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceContract_Sites_SiteId",
                table: "MaintenanceContract");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "MaintenanceContract",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftMaintenanceCompanyId",
                table: "MaintenanceContract",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}

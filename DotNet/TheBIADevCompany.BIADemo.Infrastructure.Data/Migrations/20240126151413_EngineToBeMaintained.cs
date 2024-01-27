using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EngineToBeMaintained : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AircraftMaintenanceCompanies_Teams_Id",
                table: "AircraftMaintenanceCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_Engine_Planes_PlaneId",
                table: "Engine");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTeams_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTeams_Teams_Id",
                table: "MaintenanceTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Teams_Id",
                table: "Sites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Engine",
                table: "Engine");

            migrationBuilder.RenameTable(
                name: "Engine",
                newName: "Engines");

            migrationBuilder.RenameIndex(
                name: "IX_Engine_PlaneId",
                table: "Engines",
                newName: "IX_Engines_PlaneId");

            migrationBuilder.AddColumn<bool>(
                name: "IsToBeMaintained",
                table: "Engines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Engines",
                table: "Engines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AircraftMaintenanceCompanies_Teams_Id",
                table: "AircraftMaintenanceCompanies",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTeams_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams",
                column: "AircraftMaintenanceCompanyId",
                principalTable: "AircraftMaintenanceCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTeams_Teams_Id",
                table: "MaintenanceTeams",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Teams_Id",
                table: "Sites",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AircraftMaintenanceCompanies_Teams_Id",
                table: "AircraftMaintenanceCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTeams_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTeams_Teams_Id",
                table: "MaintenanceTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Teams_Id",
                table: "Sites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Engines",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "IsToBeMaintained",
                table: "Engines");

            migrationBuilder.RenameTable(
                name: "Engines",
                newName: "Engine");

            migrationBuilder.RenameIndex(
                name: "IX_Engines_PlaneId",
                table: "Engine",
                newName: "IX_Engine_PlaneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Engine",
                table: "Engine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AircraftMaintenanceCompanies_Teams_Id",
                table: "AircraftMaintenanceCompanies",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Engine_Planes_PlaneId",
                table: "Engine",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTeams_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams",
                column: "AircraftMaintenanceCompanyId",
                principalTable: "AircraftMaintenanceCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTeams_Teams_Id",
                table: "MaintenanceTeams",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Teams_Id",
                table: "Sites",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}

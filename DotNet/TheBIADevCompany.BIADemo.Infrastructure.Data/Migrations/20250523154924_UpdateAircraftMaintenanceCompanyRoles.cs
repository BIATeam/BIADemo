using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAircraftMaintenanceCompanyRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleTeamTypes",
                keyColumns: new[] { "RolesId", "TeamTypesId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Code", "Label" },
                values: new object[] { "AircraftMaintenanceCompany_Admin", "AircraftMaintenanceCompany administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[] { 4, "MaintenanceTeam_Admin", "MaintenanceTeam administrator" });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 4, 4 });

            migrationBuilder.Sql("UPDATE MemberRole SET RoleId = 4 WHERE RoleId  = 3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleTeamTypes",
                keyColumns: new[] { "RolesId", "TeamTypesId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "RoleTeamTypes",
                keyColumns: new[] { "RolesId", "TeamTypesId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 3, 4 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Code", "Label" },
                values: new object[] { "MaintenanceTeam_Admin", "MaintenanceTeam administrator" });

            migrationBuilder.Sql("UPDATE MemberRole SET RoleId = 3 WHERE RoleId  = 4");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[] { 3, "MaintenanceTeam_Admin", "MaintenanceTeam administrator" });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 3, 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleTeamTypes",
                keyColumns: new[] { "RolesId", "TeamTypesId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

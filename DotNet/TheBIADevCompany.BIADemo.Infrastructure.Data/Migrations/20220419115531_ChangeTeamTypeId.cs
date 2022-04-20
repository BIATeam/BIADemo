using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class ChangeTeamTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 101, 2 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 102, 2 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 201, 3 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 202, 3 });


            migrationBuilder.AlterColumn<int>(
                name: "TeamTypeId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 },
                    { 101, 3 }
                });

            migrationBuilder.UpdateData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Site");

            migrationBuilder.UpdateData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "AircraftMaintenanceCompany");

            migrationBuilder.InsertData(
                table: "TeamTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "MaintenanceTeam" });


            migrationBuilder.Sql(@"UPDATE Teams SET TeamTypeId = TeamTypeId + 1");


            migrationBuilder.DeleteData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 201, 4 });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 202, 4 });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 102, 4 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 101, 3 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 102, 4 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 201, 4 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 202, 4 });


            migrationBuilder.Sql(@"UPDATE Teams SET TeamTypeId = TeamTypeId - 1");


            migrationBuilder.DeleteData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "TeamTypeId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[,]
                {
                    { 101, 2 },
                    { 102, 2 },
                    { 201, 3 },
                    { 202, 3 }
                });

            migrationBuilder.UpdateData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "AircraftMaintenanceCompany");

            migrationBuilder.UpdateData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "MaintenanceTeam");

            migrationBuilder.InsertData(
                table: "TeamTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Site" });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 2, 1 });
        }
    }
}

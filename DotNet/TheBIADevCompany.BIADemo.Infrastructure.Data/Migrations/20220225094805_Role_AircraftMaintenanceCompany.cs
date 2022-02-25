using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class Role_AircraftMaintenanceCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 101, "Team_Leader", "Team leader" },
                    { 102, "Operator", "Operator" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 101, "Team_Leader", "Team leader" },
                    { 102, "Operator", "Operator" }
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 101, 101 },
                    { 102, 102 }
                });

            migrationBuilder.InsertData(
                table: "PermissionTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 10101, "Chef d'equipe", 2, 101 },
                    { 10102, "Jefe de equipo", 3, 101 },
                    { 10103, "Teamleiter", 4, 101 },
                    { 10201, "Operateur", 2, 102 },
                    { 10202, "Operador", 3, 102 },
                    { 10203, "Operator", 4, 102 }
                });

            migrationBuilder.InsertData(
                table: "RoleTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "RoleId" },
                values: new object[,]
                {
                    { 10101, "Chef d'equipe", 2, 101 },
                    { 10102, "Jefe de equipo", 3, 101 },
                    { 10103, "Teamleiter", 4, 101 },
                    { 10201, "Operateur", 2, 102 },
                    { 10202, "Operador", 3, 102 },
                    { 10203, "Operator", 4, 102 }
                });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[,]
                {
                    { 101, 2 },
                    { 102, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 101, 101 });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 102, 102 });

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10101);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10102);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10103);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10201);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10202);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10203);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10101);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10102);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10103);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10201);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10202);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10203);

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 101, 2 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 102, 2 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 102);
        }
    }
}

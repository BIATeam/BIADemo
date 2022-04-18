using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class RootRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamTypeId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Planes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[] { 10001, "Admin", "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[] { 10002, "BackAdmin", "Background task administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[] { 10003, "BackReadOnly", "Visualization of background tasks" });

            migrationBuilder.InsertData(
                table: "RoleTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "RoleId" },
                values: new object[,]
                {
                    { 1000101, "Administrateur", 2, 10001 },
                    { 1000102, "Administrador", 3, 10001 },
                    { 1000103, "Administrator", 4, 10001 },
                    { 1000201, "Administrateur des tâches en arrière-plan", 2, 10002 },
                    { 1000202, "Administrador de tareas en segundo plano", 3, 10002 },
                    { 1000203, "Administrator für Hintergrundaufgaben", 4, 10002 },
                    { 1000301, "Visualisation des tâches en arrière-plan", 2, 10003 },
                    { 1000302, "Visualización de tareas en segundo plano", 3, 10003 },
                    { 1000303, "Visualisierung von Hintergrundaufgaben", 4, 10003 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000101);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000102);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000103);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000201);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000202);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000203);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000301);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000302);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 1000303);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10001);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10002);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10003);

            migrationBuilder.AlterColumn<int>(
                name: "TeamTypeId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

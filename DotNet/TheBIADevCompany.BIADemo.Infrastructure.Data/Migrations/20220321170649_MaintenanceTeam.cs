using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class MaintenanceTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AircraftMaintenanceCompanyId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceTeams_AircraftMaintenanceCompanies_AircraftMaintenanceCompanyId",
                        column: x => x.AircraftMaintenanceCompanyId,
                        principalTable: "AircraftMaintenanceCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceTeams_Teams_Id",
                        column: x => x.Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10101,
                column: "Label",
                value: "Superviseur");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10102,
                column: "Label",
                value: "Supervisor");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10103,
                column: "Label",
                value: "Supervisor");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10201,
                column: "Label",
                value: "Expert");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10202,
                column: "Label",
                value: "Experto");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10203,
                column: "Label",
                value: "Experte");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Supervisor", "Supervisor" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Expert", "Expert" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 201, "Team_Leader", "Team leader" },
                    { 202, "Operator", "Operator" }
                });

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10101,
                column: "Label",
                value: "Superviseur");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10102,
                column: "Label",
                value: "Supervisor");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10103,
                column: "Label",
                value: "Supervisor");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10201,
                column: "Label",
                value: "Expert");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10202,
                column: "Label",
                value: "Experto");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10203,
                column: "Label",
                value: "Experte");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Supervisor", "Supervisor" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Expert", "Expert" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 202, "Operator", "Operator" },
                    { 201, "Team_Leader", "Team leader" }
                });

            migrationBuilder.InsertData(
                table: "TeamTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "MaintenanceTeam" });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 202, 202 },
                    { 201, 201 }
                });

            migrationBuilder.InsertData(
                table: "PermissionTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 20101, "Chef d'equipe", 2, 201 },
                    { 20102, "Jefe de equipo", 3, 201 },
                    { 20103, "Teamleiter", 4, 201 },
                    { 20201, "Operateur", 2, 202 },
                    { 20202, "Operador", 3, 202 },
                    { 20203, "Operator", 4, 202 }
                });

            migrationBuilder.InsertData(
                table: "RoleTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "RoleId" },
                values: new object[,]
                {
                    { 20203, "Operator", 4, 202 },
                    { 20202, "Operador", 3, 202 },
                    { 20103, "Teamleiter", 4, 201 },
                    { 20102, "Jefe de equipo", 3, 201 },
                    { 20101, "Chef d'equipe", 2, 201 },
                    { 20201, "Operateur", 2, 202 }
                });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[,]
                {
                    { 202, 3 },
                    { 201, 3 },
                    { 102, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeams_AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams",
                column: "AircraftMaintenanceCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceTeams");

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 201, 201 });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 202, 202 });

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 20101);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 20102);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 20103);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 20201);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 20202);

            migrationBuilder.DeleteData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 20203);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 20101);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 20102);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 20103);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 20201);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 20202);

            migrationBuilder.DeleteData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 20203);

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 102, 3 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 201, 3 });

            migrationBuilder.DeleteData(
                table: "TeamTypeRole",
                keyColumns: new[] { "RoleId", "TeamTypeId" },
                keyValues: new object[] { 202, 3 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10101,
                column: "Label",
                value: "Chef d'equipe");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10102,
                column: "Label",
                value: "Jefe de equipo");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10103,
                column: "Label",
                value: "Teamleiter");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10201,
                column: "Label",
                value: "Operateur");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10202,
                column: "Label",
                value: "Operador");

            migrationBuilder.UpdateData(
                table: "PermissionTranslations",
                keyColumn: "Id",
                keyValue: 10203,
                column: "Label",
                value: "Operator");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Team_Leader", "Team leader" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Operator", "Operator" });

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10101,
                column: "Label",
                value: "Chef d'equipe");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10102,
                column: "Label",
                value: "Jefe de equipo");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10103,
                column: "Label",
                value: "Teamleiter");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10201,
                column: "Label",
                value: "Operateur");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10202,
                column: "Label",
                value: "Operador");

            migrationBuilder.UpdateData(
                table: "RoleTranslations",
                keyColumn: "Id",
                keyValue: 10203,
                column: "Label",
                value: "Operator");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Team_Leader", "Team leader" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Code", "Label" },
                values: new object[] { "Operator", "Operator" });
        }
    }
}

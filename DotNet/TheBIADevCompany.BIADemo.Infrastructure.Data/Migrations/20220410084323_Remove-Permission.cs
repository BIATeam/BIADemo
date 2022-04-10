using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class RemovePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "PermissionTranslations");

            migrationBuilder.DropTable(
                name: "Permissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionTranslations_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 1, "Site_Admin", "Airline administrator" },
                    { 2, "Pilot", "Pilot" },
                    { 101, "Supervisor", "Supervisor" },
                    { 102, "Expert", "Expert" },
                    { 201, "Team_Leader", "Team leader" },
                    { 202, "Operator", "Operator" }
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 101, 101 },
                    { 201, 201 },
                    { 102, 102 },
                    { 2, 2 },
                    { 202, 202 },
                    { 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "PermissionTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 202, "Piloto", 3, 2 },
                    { 20202, "Operador", 3, 202 },
                    { 20201, "Operateur", 2, 202 },
                    { 102, "Administrador de la aerolínea", 3, 1 },
                    { 20103, "Teamleiter", 4, 201 },
                    { 20102, "Jefe de equipo", 3, 201 },
                    { 20101, "Chef d'equipe", 2, 201 },
                    { 103, "Fluglinienadministrator", 4, 1 },
                    { 10203, "Experte", 4, 102 },
                    { 10202, "Experto", 3, 102 },
                    { 10201, "Expert", 2, 102 },
                    { 20203, "Operator", 4, 202 },
                    { 10103, "Supervisor", 4, 101 },
                    { 10102, "Supervisor", 3, 101 },
                    { 10101, "Superviseur", 2, 101 },
                    { 203, "Pilot", 4, 2 },
                    { 201, "Pilote", 2, 2 },
                    { 101, "Administrateur de la compagnie", 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleId",
                table: "PermissionRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTranslations_LanguageId",
                table: "PermissionTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTranslations_PermissionId_LanguageId",
                table: "PermissionTranslations",
                columns: new[] { "PermissionId", "LanguageId" },
                unique: true);
        }
    }
}

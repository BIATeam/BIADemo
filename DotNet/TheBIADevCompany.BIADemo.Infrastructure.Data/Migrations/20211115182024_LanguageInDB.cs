using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class LanguageInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Roles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "NotificationTypes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "NotificationTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypeTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTypeTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTypeTranslations_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTranslations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "EN", "English" },
                    { 2, "FR", "Français" },
                    { 3, "ES", "Española" },
                    { 4, "DE", "Deutsch" }
                });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Label",
                value: "Task");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Label",
                value: "Info");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Label",
                value: "Success");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Label",
                value: "Warn");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Label",
                value: "Error");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Label",
                value: "Airline administrator");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Label",
                value: "Pilot");

            migrationBuilder.InsertData(
                table: "NotificationTypeTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "NotificationTypeId" },
                values: new object[,]
                {
                    { 101, "Tâche", 2, 1 },
                    { 503, "Fehler", 4, 5 },
                    { 403, "Erwärmen", 4, 4 },
                    { 303, "Erfolg", 4, 3 },
                    { 203, "Information", 4, 2 },
                    { 103, "Aufgabe", 4, 1 },
                    { 502, "Culpa", 3, 5 },
                    { 302, "Éxito", 3, 3 },
                    { 402, "Advertencia", 3, 4 },
                    { 102, "Tarea", 3, 1 },
                    { 501, "Erreur", 2, 5 },
                    { 401, "Avertissement", 2, 4 },
                    { 301, "Succès", 2, 3 },
                    { 201, "Information", 2, 2 },
                    { 202, "Información", 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "RoleTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "RoleId" },
                values: new object[,]
                {
                    { 103, "Fluglinienadministrator", 4, 1 },
                    { 201, "Pilote", 2, 2 },
                    { 102, "Administrador de la aerolínea", 3, 1 },
                    { 202, "Piloto", 3, 2 },
                    { 101, "Administrateur de la compagnie", 2, 1 },
                    { 203, "Pilot", 4, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslations_LanguageId",
                table: "NotificationTypeTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslations_NotificationTypeId",
                table: "NotificationTypeTranslations",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTranslations_LanguageId",
                table: "RoleTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTranslations_RoleId",
                table: "RoleTranslations",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTypeTranslations");

            migrationBuilder.DropTable(
                name: "RoleTranslations");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "NotificationTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "NotificationTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class Translationforpermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Permission",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PermissionTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionTranslation_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionTranslation_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1,
                column: "Label",
                value: "Airline administrator");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2,
                column: "Label",
                value: "Pilot");

            migrationBuilder.InsertData(
                table: "PermissionTranslation",
                columns: new[] { "Id", "Label", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 101, "Administrateur de la compagnie", 2, 1 },
                    { 102, "Administrador de la aerolínea", 3, 1 },
                    { 103, "Fluglinienadministrator", 4, 1 },
                    { 201, "Pilote", 2, 2 },
                    { 202, "Piloto", 3, 2 },
                    { 203, "Pilot", 4, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTranslation_LanguageId",
                table: "PermissionTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTranslation_PermissionId_LanguageId",
                table: "PermissionTranslation",
                columns: new[] { "PermissionId", "LanguageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionTranslation");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Permission");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class Translationfornotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoleTranslations_RoleId",
                table: "RoleTranslations");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTypeTranslations_NotificationTypeId",
                table: "NotificationTypeTranslations");

            migrationBuilder.CreateTable(
                name: "NotificationTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTranslation_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTranslation_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleTranslations_RoleId_LanguageId",
                table: "RoleTranslations",
                columns: new[] { "RoleId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslations_NotificationTypeId_LanguageId",
                table: "NotificationTypeTranslations",
                columns: new[] { "NotificationTypeId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTranslation_LanguageId",
                table: "NotificationTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTranslation_NotificationId_LanguageId",
                table: "NotificationTranslation",
                columns: new[] { "NotificationId", "LanguageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTranslation");

            migrationBuilder.DropIndex(
                name: "IX_RoleTranslations_RoleId_LanguageId",
                table: "RoleTranslations");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTypeTranslations_NotificationTypeId_LanguageId",
                table: "NotificationTypeTranslations");

            migrationBuilder.DropIndex(
                name: "IX_Languages_Code",
                table: "Languages");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTranslations_RoleId",
                table: "RoleTranslations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslations_NotificationTypeId",
                table: "NotificationTypeTranslations",
                column: "NotificationTypeId");
        }
    }
}

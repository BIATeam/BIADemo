using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class NotificationRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Sites_SiteId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SiteId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Notifications");

            migrationBuilder.CreateTable(
                name: "NotificationRole",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationRole", x => new { x.RoleId, x.NotificationId });
                    table.ForeignKey(
                        name: "FK_NotificationRole_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRole_NotificationId",
                table: "NotificationRole",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationRole");

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SiteId",
                table: "Notifications",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Sites_SiteId",
                table: "Notifications",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class ChangeNotifModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Roles_NotifiedRoleId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotifiedRoleId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Read",
                table: "NotificationUser");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "NotifiedRoleId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "TargetRoute",
                table: "Notification",
                newName: "TargetJson");

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
                        name: "FK_NotificationRole_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
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

            migrationBuilder.RenameColumn(
                name: "TargetJson",
                table: "Notification",
                newName: "TargetRoute");

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "NotificationUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotifiedRoleId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotifiedRoleId",
                table: "Notification",
                column: "NotifiedRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Roles_NotifiedRoleId",
                table: "Notification",
                column: "NotifiedRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

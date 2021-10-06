using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class DBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationType_TypeId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Sites_SiteId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_CreatedById",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationPermission_Notification_NotificationId",
                table: "NotificationPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUser_Notification_NotificationId",
                table: "NotificationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationType",
                newName: "NotificationTypes");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_TypeId",
                table: "Notifications",
                newName: "IX_Notifications_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SiteId",
                table: "Notifications",
                newName: "IX_Notifications_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_CreatedById",
                table: "Notifications",
                newName: "IX_Notifications_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationPermission_Notifications_NotificationId",
                table: "NotificationPermission",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_TypeId",
                table: "Notifications",
                column: "TypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Sites_SiteId",
                table: "Notifications",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_CreatedById",
                table: "Notifications",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUser_Notifications_NotificationId",
                table: "NotificationUser",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationPermission_Notifications_NotificationId",
                table: "NotificationPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_TypeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Sites_SiteId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_CreatedById",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUser_Notifications_NotificationId",
                table: "NotificationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "NotificationTypes",
                newName: "NotificationType");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_TypeId",
                table: "Notification",
                newName: "IX_Notification_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_SiteId",
                table: "Notification",
                newName: "IX_Notification_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CreatedById",
                table: "Notification",
                newName: "IX_Notification_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationType_TypeId",
                table: "Notification",
                column: "TypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Sites_SiteId",
                table: "Notification",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_CreatedById",
                table: "Notification",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationPermission_Notification_NotificationId",
                table: "NotificationPermission",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUser_Notification_NotificationId",
                table: "NotificationUser",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

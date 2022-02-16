using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class DBSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationPermission_Permission_PermissionId",
                table: "NotificationPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTranslation_Languages_LanguageId",
                table: "NotificationTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTranslation_Notifications_NotificationId",
                table: "NotificationTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_Permission_PermissionId",
                table: "PermissionRole");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionTranslation_Languages_LanguageId",
                table: "PermissionTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionTranslation_Permission_PermissionId",
                table: "PermissionTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionTranslation",
                table: "PermissionTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTranslation",
                table: "NotificationTranslation");

            migrationBuilder.RenameTable(
                name: "PermissionTranslation",
                newName: "PermissionTranslations");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "NotificationTranslation",
                newName: "NotificationTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionTranslation_PermissionId_LanguageId",
                table: "PermissionTranslations",
                newName: "IX_PermissionTranslations_PermissionId_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionTranslation_LanguageId",
                table: "PermissionTranslations",
                newName: "IX_PermissionTranslations_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationTranslation_NotificationId_LanguageId",
                table: "NotificationTranslations",
                newName: "IX_NotificationTranslations_NotificationId_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationTranslation_LanguageId",
                table: "NotificationTranslations",
                newName: "IX_NotificationTranslations_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionTranslations",
                table: "PermissionTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTranslations",
                table: "NotificationTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationPermission_Permissions_PermissionId",
                table: "NotificationPermission",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTranslations_Languages_LanguageId",
                table: "NotificationTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTranslations_Notifications_NotificationId",
                table: "NotificationTranslations",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_Permissions_PermissionId",
                table: "PermissionRole",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionTranslations_Languages_LanguageId",
                table: "PermissionTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionTranslations_Permissions_PermissionId",
                table: "PermissionTranslations",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationPermission_Permissions_PermissionId",
                table: "NotificationPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTranslations_Languages_LanguageId",
                table: "NotificationTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTranslations_Notifications_NotificationId",
                table: "NotificationTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_Permissions_PermissionId",
                table: "PermissionRole");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionTranslations_Languages_LanguageId",
                table: "PermissionTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionTranslations_Permissions_PermissionId",
                table: "PermissionTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionTranslations",
                table: "PermissionTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTranslations",
                table: "NotificationTranslations");

            migrationBuilder.RenameTable(
                name: "PermissionTranslations",
                newName: "PermissionTranslation");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "NotificationTranslations",
                newName: "NotificationTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionTranslations_PermissionId_LanguageId",
                table: "PermissionTranslation",
                newName: "IX_PermissionTranslation_PermissionId_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionTranslations_LanguageId",
                table: "PermissionTranslation",
                newName: "IX_PermissionTranslation_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationTranslations_NotificationId_LanguageId",
                table: "NotificationTranslation",
                newName: "IX_NotificationTranslation_NotificationId_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationTranslations_LanguageId",
                table: "NotificationTranslation",
                newName: "IX_NotificationTranslation_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionTranslation",
                table: "PermissionTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTranslation",
                table: "NotificationTranslation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationPermission_Permission_PermissionId",
                table: "NotificationPermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTranslation_Languages_LanguageId",
                table: "NotificationTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTranslation_Notifications_NotificationId",
                table: "NotificationTranslation",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_Permission_PermissionId",
                table: "PermissionRole",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionTranslation_Languages_LanguageId",
                table: "PermissionTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionTranslation_Permission_PermissionId",
                table: "PermissionTranslation",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

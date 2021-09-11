using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class RemoveNotifType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationType_TypeId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "NotificationType");

            migrationBuilder.DropIndex(
                name: "IX_Notification_TypeId",
                table: "Notification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NotificationType",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 1, "Task" },
                    { 2, "Info" },
                    { 3, "Success" },
                    { 4, "Warning" },
                    { 5, "Error" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TypeId",
                table: "Notification",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationType_TypeId",
                table: "Notification",
                column: "TypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

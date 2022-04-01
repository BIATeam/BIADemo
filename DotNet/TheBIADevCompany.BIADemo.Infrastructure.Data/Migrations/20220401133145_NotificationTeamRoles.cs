using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class NotificationTeamRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTeam",
                table: "NotificationTeam");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "NotificationTeam",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTeam",
                table: "NotificationTeam",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "NotificationTeamRole",
                columns: table => new
                {
                    NotificationTeamId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTeamRole", x => new { x.NotificationTeamId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_NotificationTeamRole_NotificationTeam_NotificationTeamId",
                        column: x => x.NotificationTeamId,
                        principalTable: "NotificationTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTeamRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTeam_TeamId",
                table: "NotificationTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTeamRole_RoleId",
                table: "NotificationTeamRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTeamRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTeam",
                table: "NotificationTeam");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTeam_TeamId",
                table: "NotificationTeam");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "NotificationTeam");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTeam",
                table: "NotificationTeam",
                columns: new[] { "TeamId", "NotificationId" });
        }
    }
}

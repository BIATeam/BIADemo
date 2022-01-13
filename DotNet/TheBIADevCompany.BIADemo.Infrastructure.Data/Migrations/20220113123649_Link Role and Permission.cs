using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class LinkRoleandPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { 2, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleId",
                table: "PermissionRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRole");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class RolesByTeamId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamTypeRole");

            migrationBuilder.CreateTable(
                name: "RoleTeamTypes",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    TeamTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTeamTypes", x => new { x.RolesId, x.TeamTypesId });
                    table.ForeignKey(
                        name: "FK_RoleTeamTypes_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTeamTypes_TeamTypes_TeamTypesId",
                        column: x => x.TeamTypesId,
                        principalTable: "TeamTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 },
                    { 101, 3 },
                    { 102, 3 },
                    { 201, 4 },
                    { 202, 4 },
                    { 102, 4 }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10002,
                column: "Code",
                value: "Back_Admin");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10003,
                column: "Code",
                value: "Back_Read_Only");

            migrationBuilder.InsertData(
                table: "TeamTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Root" });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 10001, 1 });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 10002, 1 });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[] { 10003, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RoleTeamTypes_TeamTypesId",
                table: "RoleTeamTypes",
                column: "TeamTypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleTeamTypes");

            migrationBuilder.DeleteData(
                table: "TeamTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "TeamTypeRole",
                columns: table => new
                {
                    TeamTypeId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTypeRole", x => new { x.TeamTypeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_TeamTypeRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTypeRole_TeamTypes_TeamTypeId",
                        column: x => x.TeamTypeId,
                        principalTable: "TeamTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10002,
                column: "Code",
                value: "BackAdmin");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10003,
                column: "Code",
                value: "BackReadOnly");

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 },
                    { 101, 3 },
                    { 102, 3 },
                    { 201, 4 },
                    { 202, 4 },
                    { 102, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamTypeRole_RoleId",
                table: "TeamTypeRole",
                column: "RoleId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Sites_SiteId",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "Members",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_SiteId_UserId",
                table: "Members",
                newName: "IX_Members_TeamId_UserId");

            // remove manualy

#pragma warning disable S125 // Sections of code should not be commented out
            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "Sites",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");
#pragma warning disable S125 // Sections of code should not be commented out

            // end remove manualy

            // added manualy
            migrationBuilder.DropForeignKey(
                name: "FK_ViewSite_Sites_SiteId",
                table: "ViewSite");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Sites_SiteId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Sites_SiteId",
                table: "Planes");

            // ... Add the drop to other foreign key 

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sites",
                table: "Sites");

            migrationBuilder.CreateTable(
                name: "TmpSites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                }
                ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                }
                );


            migrationBuilder.Sql(@"
                INSERT INTO[dbo].[TmpSites] (Id, Title) SELECT Id, Title FROM[dbo].[Sites]
                DROP TABLE [dbo].[Sites]
                EXECUTE sp_rename N'TmpSites', N'Sites'
            ");

            // ... Add the creation to other foreign key 

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Sites_SiteId",
                table: "Planes",
                column: "SiteId",
                principalTable: "Sites",
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
                name: "FK_ViewSite_Sites_SiteId",
                table: "ViewSite",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // end added manualy

            migrationBuilder.CreateTable(
                name: "TeamTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamTypeId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_TeamTypes_TeamTypeId",
                        column: x => x.TeamTypeId,
                        principalTable: "TeamTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


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

            migrationBuilder.InsertData(
                table: "TeamTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Site" });

            // added manualy after InsertData("TeamTypes"
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [dbo].[Teams] ON
                INSERT INTO[dbo].[Teams] (Id, TeamTypeId) SELECT Id, '1' FROM[dbo].[Sites]
                SET IDENTITY_INSERT [dbo].[Teams] OFF
            ");
            // end added manualy

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "TeamTypeRole",
                columns: new[] { "RoleId", "TeamTypeId" },
                values: new object[] { 2, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamTypeId",
                table: "Teams",
                column: "TeamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTypeRole_RoleId",
                table: "TeamTypeRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Teams_TeamId",
                table: "Members",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Teams_Id",
                table: "Sites",
                column: "Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Teams_TeamId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Teams_Id",
                table: "Sites");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "TeamTypeRole");

            migrationBuilder.DropTable(
                name: "TeamTypes");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Members",
                newName: "SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_TeamId_UserId",
                table: "Members",
                newName: "IX_Members_SiteId_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Sites_SiteId",
                table: "Members",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

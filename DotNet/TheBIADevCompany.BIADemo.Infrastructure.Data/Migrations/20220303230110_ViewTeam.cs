using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class ViewTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ViewTeam",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    ViewId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewTeam", x => new { x.TeamId, x.ViewId });
                    table.ForeignKey(
                        name: "FK_ViewTeam_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViewTeam_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.Sql(@"
                INSERT INTO[dbo].[ViewTeam] (TeamId, ViewId, IsDefault) SELECT SiteId, ViewId, IsDefault FROM[dbo].[ViewSite]
            ");

            migrationBuilder.DropTable(
                name: "ViewSite");

            migrationBuilder.CreateIndex(
                name: "IX_ViewTeam_ViewId",
                table: "ViewTeam",
                column: "ViewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewTeam");

            migrationBuilder.CreateTable(
                name: "ViewSite",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    ViewId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewSite", x => new { x.SiteId, x.ViewId });
                    table.ForeignKey(
                        name: "FK_ViewSite_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViewSite_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewSite_ViewId",
                table: "ViewSite",
                column: "ViewId");
        }
    }
}

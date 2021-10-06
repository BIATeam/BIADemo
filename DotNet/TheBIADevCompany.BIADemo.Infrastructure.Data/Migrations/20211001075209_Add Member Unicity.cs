using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class AddMemberUnicity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_SiteId",
                table: "Members");

            migrationBuilder.CreateIndex(
                name: "IX_Members_SiteId_UserId",
                table: "Members",
                columns: new[] { "SiteId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_SiteId_UserId",
                table: "Members");

            migrationBuilder.CreateIndex(
                name: "IX_Members_SiteId",
                table: "Members",
                column: "SiteId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SiteUniqueIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "Sites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_UniqueIdentifier",
                table: "Sites",
                column: "UniqueIdentifier",
                unique: true,
                filter: "[UniqueIdentifier] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sites_UniqueIdentifier",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "Sites");
        }
    }
}

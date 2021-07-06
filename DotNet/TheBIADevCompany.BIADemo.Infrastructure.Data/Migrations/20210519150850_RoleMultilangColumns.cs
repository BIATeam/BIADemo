using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class RoleMultilangColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Label",
                table: "Roles",
                newName: "LabelEn");

            migrationBuilder.AddColumn<string>(
                name: "LabelEs",
                table: "Roles",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelFr",
                table: "Roles",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LabelEn",
                table: "Roles",
                newName: "Label");

            migrationBuilder.DropColumn(
                name: "LabelEs",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LabelFr",
                table: "Roles");
        }
    }
}

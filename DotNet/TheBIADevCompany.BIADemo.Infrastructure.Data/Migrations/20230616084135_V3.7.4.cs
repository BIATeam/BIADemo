using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class V374 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login_Domain",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "--",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "--");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Roles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "--",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "--");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Roles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login_Domain",
                table: "Users",
                columns: new[] { "Login", "Domain" },
                unique: true);
        }
    }
}

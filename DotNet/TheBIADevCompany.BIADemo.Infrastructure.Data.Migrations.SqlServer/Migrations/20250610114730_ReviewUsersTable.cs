using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ReviewUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DistinguishedName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExternalCompany",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmployee",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubDepartment",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "DaiDate",
                table: "Users",
                newName: "LastSyncDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSyncDate",
                table: "Users",
                newName: "DaiDate");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistinguishedName",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalCompany",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmployee",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubDepartment",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}

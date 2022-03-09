using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class Title_In_Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Teams",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                defaultValue: "");

            migrationBuilder.Sql(@"
                MERGE INTO [dbo].[Teams] T
                   USING [dbo].[Sites] S 
                      ON T.id = S.id
                WHEN MATCHED THEN
                   UPDATE 
                      SET Title = S.Title;
            ");

            migrationBuilder.Sql(@"
                MERGE INTO [dbo].[Teams] T
                   USING [dbo].[AircraftMaintenanceCompanies] S 
                      ON T.id = S.id
                WHEN MATCHED THEN
                   UPDATE 
                      SET Title = S.Name;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Teams",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AircraftMaintenanceCompanies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Sites",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AircraftMaintenanceCompanies",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}

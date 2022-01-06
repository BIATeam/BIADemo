using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class ViewTranslated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: -1,
                column: "Preference",
                value: "{\"first\":0,\"rows\":10,\"sortField\":\"createdDate\",\"sortOrder\":-1,\"columnOrder\":[\"titleTranslated\",\"descriptionTranslated\",\"type\",\"read\",\"createdDate\",\"createdBy\"],\"selection\":[],\"filters\":{}}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: -1,
                column: "Preference",
                value: "{\"first\":0,\"rows\":10,\"sortField\":\"createdDate\",\"sortOrder\":-1,\"columnOrder\":[\"title\",\"description\",\"type\",\"read\",\"createdDate\",\"createdBy\"],\"selection\":[],\"filters\":{}}");
        }
    }
}

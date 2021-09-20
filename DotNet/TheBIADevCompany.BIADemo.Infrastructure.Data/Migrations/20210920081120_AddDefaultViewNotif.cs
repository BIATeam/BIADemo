using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class AddDefaultViewNotif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Description", "Name", "Preference", "TableId", "ViewType" },
                values: new object[] { -1, null, "default", "{\"first\":0,\"rows\":10,\"sortField\":\"createdDate\",\"sortOrder\":-1,\"columnOrder\":[\"title\",\"description\",\"type\",\"read\",\"createdDate\",\"createdBy\"],\"selection\":[],\"filters\":{}}'", "notificationsGrid", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}

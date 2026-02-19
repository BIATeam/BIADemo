using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationTypeDownloadReady : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[] { 6, "downloadr", "Download Ready" });

            migrationBuilder.InsertData(
                table: "NotificationTypeTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "NotificationTypeId" },
                values: new object[,]
                {
                    { 601, "Téléchargement prêt", 2, 6 },
                    { 602, "Descarga lista", 3, 6 },
                    { 603, "Download bereit", 4, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTypeTranslations",
                keyColumn: "Id",
                keyValue: 601);

            migrationBuilder.DeleteData(
                table: "NotificationTypeTranslations",
                keyColumn: "Id",
                keyValue: 602);

            migrationBuilder.DeleteData(
                table: "NotificationTypeTranslations",
                keyColumn: "Id",
                keyValue: 603);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}

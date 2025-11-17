using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerMessageTypeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "BannerMessages",
                newName: "TypeId");

            migrationBuilder.CreateTable(
                name: "BannerMessageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerMessageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannerMessageTypeTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    BannerMessageTypeId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerMessageTypeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerMessageTypeTranslations_BannerMessageTypes_BannerMessageTypeId",
                        column: x => x.BannerMessageTypeId,
                        principalTable: "BannerMessageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerMessageTypeTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BannerMessageTypes",
                column: "Id",
                values: new object[]
                {
                    0,
                    1
                });

            migrationBuilder.InsertData(
                table: "BannerMessageTypeTranslations",
                columns: new[] { "Id", "BannerMessageTypeId", "Label", "LanguageId" },
                values: new object[,]
                {
                    { 101, 0, "Information", 1 },
                    { 102, 1, "Warning", 1 },
                    { 103, 0, "Information", 2 },
                    { 104, 1, "Avertissement", 2 },
                    { 105, 0, "Información", 3 },
                    { 106, 1, "Advertencia", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannerMessages_TypeId",
                table: "BannerMessages",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerMessageTypeTranslations_BannerMessageTypeId_LanguageId",
                table: "BannerMessageTypeTranslations",
                columns: new[] { "BannerMessageTypeId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannerMessageTypeTranslations_LanguageId",
                table: "BannerMessageTypeTranslations",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BannerMessages_BannerMessageTypes_TypeId",
                table: "BannerMessages",
                column: "TypeId",
                principalTable: "BannerMessageTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannerMessages_BannerMessageTypes_TypeId",
                table: "BannerMessages");

            migrationBuilder.DropTable(
                name: "BannerMessageTypeTranslations");

            migrationBuilder.DropTable(
                name: "BannerMessageTypes");

            migrationBuilder.DropIndex(
                name: "IX_BannerMessages_TypeId",
                table: "BannerMessages");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "BannerMessages",
                newName: "Type");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnoucementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnoucementAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditChanges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditUserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoucementAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "AnnoucementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoucementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Annoucements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    RawContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annoucements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Annoucements_AnnoucementTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AnnoucementTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnnoucementTypeTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    AnnoucementTypeId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoucementTypeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnoucementTypeTranslations_AnnoucementTypes_AnnoucementTypeId",
                        column: x => x.AnnoucementTypeId,
                        principalTable: "AnnoucementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnoucementTypeTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AnnoucementTypes",
                column: "Id",
                values: new object[]
                {
                    0,
                    1
                });

            migrationBuilder.InsertData(
                table: "AnnoucementTypeTranslations",
                columns: new[] { "Id", "AnnoucementTypeId", "Label", "LanguageId" },
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
                name: "IX_Annoucements_TypeId",
                table: "Annoucements",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnoucementTypeTranslations_AnnoucementTypeId_LanguageId",
                table: "AnnoucementTypeTranslations",
                columns: new[] { "AnnoucementTypeId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnoucementTypeTranslations_LanguageId",
                table: "AnnoucementTypeTranslations",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnoucementAudit");

            migrationBuilder.DropTable(
                name: "Annoucements");

            migrationBuilder.DropTable(
                name: "AnnoucementTypeTranslations");

            migrationBuilder.DropTable(
                name: "AnnoucementTypes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Pilots",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AlterColumn<string>(
                name: "ContractNumber",
                table: "MaintenanceContract",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");

            migrationBuilder.CreateTable(
                name: "AnnouncementAudit",
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
                    table.PrimaryKey("PK_AnnouncementAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
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
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_AnnouncementTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AnnouncementTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementTypeTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    AnnouncementTypeId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementTypeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementTypeTranslations_AnnouncementTypes_AnnouncementTypeId",
                        column: x => x.AnnouncementTypeId,
                        principalTable: "AnnouncementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementTypeTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AnnouncementTypes",
                column: "Id",
                values: new object[]
                {
                    0,
                    1
                });

            migrationBuilder.InsertData(
                table: "AnnouncementTypeTranslations",
                columns: new[] { "Id", "AnnouncementTypeId", "Label", "LanguageId" },
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
                name: "IX_Announcements_TypeId",
                table: "Announcements",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementTypeTranslations_AnnouncementTypeId_LanguageId",
                table: "AnnouncementTypeTranslations",
                columns: new[] { "AnnouncementTypeId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementTypeTranslations_LanguageId",
                table: "AnnouncementTypeTranslations",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementAudit");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "AnnouncementTypeTranslations");

            migrationBuilder.DropTable(
                name: "AnnouncementTypes");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Pilots",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ContractNumber",
                table: "MaintenanceContract",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CS_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);
        }
    }
}

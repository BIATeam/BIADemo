using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.MigrationsPostGreSql
{
    /// <inheritdoc />
    public partial class SiteUniqueIdentifier_IsRoleFromApi_UpdateAuditEntities_AddAnnouncementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "Sites",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromRoleApi",
                table: "MemberRole",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AnnouncementAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AuditAction = table.Column<string>(type: "text", nullable: false),
                    AuditChanges = table.Column<string>(type: "text", nullable: false),
                    AuditUserLogin = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    RawContent = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    AnnouncementTypeId = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementTypeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementTypeTranslations_AnnouncementTypes_Announcement~",
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
                name: "IX_Sites_UniqueIdentifier",
                table: "Sites",
                column: "UniqueIdentifier",
                unique: true);

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

            migrationBuilder.DropIndex(
                name: "IX_Sites_UniqueIdentifier",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "IsFromRoleApi",
                table: "MemberRole");
        }
    }
}

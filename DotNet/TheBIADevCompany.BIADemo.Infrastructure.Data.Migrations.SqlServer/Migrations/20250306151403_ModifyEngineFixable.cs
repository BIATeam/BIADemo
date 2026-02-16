using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEngineFixable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FixedDate",
                table: "Engines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "Engines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedDate",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "Engines");
        }
    }
}

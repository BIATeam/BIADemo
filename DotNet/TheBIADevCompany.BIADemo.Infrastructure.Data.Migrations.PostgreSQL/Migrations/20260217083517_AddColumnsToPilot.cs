using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.MigrationsPostGreSql
{
    /// <inheritdoc />
    public partial class AddColumnsToPilot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Pilots");

            migrationBuilder.AddColumn<int>(
                name: "BaseAirportId",
                table: "Pilots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Pilots",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CPLDate",
                table: "Pilots",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Pilots",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Pilots",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Pilots_BaseAirportId",
                table: "Pilots",
                column: "BaseAirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pilots_Airports_BaseAirportId",
                table: "Pilots",
                column: "BaseAirportId",
                principalTable: "Airports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pilots_Airports_BaseAirportId",
                table: "Pilots");

            migrationBuilder.DropIndex(
                name: "IX_Pilots_BaseAirportId",
                table: "Pilots");

            migrationBuilder.DropColumn(
                name: "BaseAirportId",
                table: "Pilots");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Pilots");

            migrationBuilder.DropColumn(
                name: "CPLDate",
                table: "Pilots");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Pilots");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Pilots");
        }
    }
}

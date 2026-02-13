using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
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
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Pilots",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CPLDate",
                table: "Pilots",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Pilots",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Pilots",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Pilots_BaseAirportId",
                table: "Pilots",
                column: "BaseAirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pilots_Airports_BaseAirportId",
                table: "Pilots",
                column: "BaseAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToPlanes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstFlightDate",
                table: "Planes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "FuelCapacity",
                table: "Planes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaintenance",
                table: "Planes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Planes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotorsCount",
                table: "Planes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextMaintenanceDate",
                table: "Planes",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Planes",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SyncFlightDataTime",
                table: "Planes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<double>(
                name: "TotalFlightHours",
                table: "Planes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstFlightDate",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "FuelCapacity",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "IsMaintenance",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "MotorsCount",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "NextMaintenanceDate",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "SyncFlightDataTime",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "TotalFlightHours",
                table: "Planes");
        }
    }
}

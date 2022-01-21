using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class DateAndTimeOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstFlightDate",
                table: "Planes");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Planes",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SyncTime",
                table: "Planes",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "SyncTime",
                table: "Planes");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstFlightDate",
                table: "Planes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

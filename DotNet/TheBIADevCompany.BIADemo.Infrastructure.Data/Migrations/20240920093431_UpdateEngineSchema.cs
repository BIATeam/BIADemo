using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEngineSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastMaintenanceDate",
                table: "Engines",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<double>(
                name: "AverageFlightHours",
                table: "Engines",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AverageFuelConsumption",
                table: "Engines",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Engines",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedPrice",
                table: "Engines",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeDate",
                table: "Engines",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FlightHours",
                table: "Engines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "FuelConsumption",
                table: "Engines",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "IgnitionTime",
                table: "Engines",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHybrid",
                table: "Engines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Engines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextMaintenanceDate",
                table: "Engines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NoiseLevel",
                table: "Engines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Engines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "AverageFlightHours",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "AverageFuelConsumption",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "EstimatedPrice",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "ExchangeDate",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "FlightHours",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "FuelConsumption",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "IgnitionTime",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "IsHybrid",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "NextMaintenanceDate",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "NoiseLevel",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Engines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastMaintenanceDate",
                table: "Engines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

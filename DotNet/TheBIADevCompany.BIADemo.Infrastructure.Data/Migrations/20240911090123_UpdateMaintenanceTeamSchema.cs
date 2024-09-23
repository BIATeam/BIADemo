using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaintenanceTeamSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM MaintenanceTeams");
            migrationBuilder.Sql($"DELETE M FROM Members as M INNER JOIN Teams AS T ON T.Id = M.TeamId WHERE T.TeamTypeId = {(int)TeamTypeId.MaintenanceTeam}");
            migrationBuilder.Sql($"DELETE NT FROM NotificationTeam as NT INNER JOIN Teams AS T ON T.Id = NT.TeamId WHERE T.TeamTypeId = {(int)TeamTypeId.MaintenanceTeam}");
            migrationBuilder.Sql($"DELETE FROM Teams WHERE TeamTypeId = {(int)TeamTypeId.MaintenanceTeam}");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "MaintenanceTeams",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "MaintenanceTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageOperationCost",
                table: "MaintenanceTeams",
                type: "Money",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageOperationDuration",
                table: "MaintenanceTeams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AverageTravelDuration",
                table: "MaintenanceTeams",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentAirportId",
                table: "MaintenanceTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentCountryId",
                table: "MaintenanceTeams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstOperation",
                table: "MaintenanceTeams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IncidentCount",
                table: "MaintenanceTeams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "MaintenanceTeams",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOperation",
                table: "MaintenanceTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxOperationDuration",
                table: "MaintenanceTeams",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxTravelDuration",
                table: "MaintenanceTeams",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextOperation",
                table: "MaintenanceTeams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OperationCount",
                table: "MaintenanceTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalOperationCost",
                table: "MaintenanceTeams",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "TotalOperationDuration",
                table: "MaintenanceTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "TotalTravelDuration",
                table: "MaintenanceTeams",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceTeamAirport",
                columns: table => new
                {
                    MaintenanceTeamId = table.Column<int>(type: "int", nullable: false),
                    AirportId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceTeamAirport", x => new { x.AirportId, x.MaintenanceTeamId });
                    table.ForeignKey(
                        name: "FK_MaintenanceTeamAirport_Airports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceTeamAirport_MaintenanceTeams_MaintenanceTeamId",
                        column: x => x.MaintenanceTeamId,
                        principalTable: "MaintenanceTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceTeamCountry",
                columns: table => new
                {
                    MaintenanceTeamId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceTeamCountry", x => new { x.CountryId, x.MaintenanceTeamId });
                    table.ForeignKey(
                        name: "FK_MaintenanceTeamCountry_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceTeamCountry_MaintenanceTeams_MaintenanceTeamId",
                        column: x => x.MaintenanceTeamId,
                        principalTable: "MaintenanceTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "France" },
                    { 2, "Mexico" },
                    { 3, "China" },
                    { 4, "Spain" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeams_CurrentAirportId",
                table: "MaintenanceTeams",
                column: "CurrentAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeams_CurrentCountryId",
                table: "MaintenanceTeams",
                column: "CurrentCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeamAirport_MaintenanceTeamId",
                table: "MaintenanceTeamAirport",
                column: "MaintenanceTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeamCountry_MaintenanceTeamId",
                table: "MaintenanceTeamCountry",
                column: "MaintenanceTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTeams_Airports_CurrentAirportId",
                table: "MaintenanceTeams",
                column: "CurrentAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTeams_Countries_CurrentCountryId",
                table: "MaintenanceTeams",
                column: "CurrentCountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTeams_Airports_CurrentAirportId",
                table: "MaintenanceTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTeams_Countries_CurrentCountryId",
                table: "MaintenanceTeams");

            migrationBuilder.DropTable(
                name: "MaintenanceTeamAirport");

            migrationBuilder.DropTable(
                name: "MaintenanceTeamCountry");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTeams_CurrentAirportId",
                table: "MaintenanceTeams");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTeams_CurrentCountryId",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "AverageOperationCost",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "AverageOperationDuration",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "AverageTravelDuration",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "CurrentAirportId",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "CurrentCountryId",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "FirstOperation",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "IncidentCount",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "LastOperation",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "MaxOperationDuration",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "MaxTravelDuration",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "NextOperation",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "OperationCount",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "TotalOperationCost",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "TotalOperationDuration",
                table: "MaintenanceTeams");

            migrationBuilder.DropColumn(
                name: "TotalTravelDuration",
                table: "MaintenanceTeams");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "MaintenanceTeams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAirportRelationshipTablePlane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM PlaneAirport");
            migrationBuilder.Sql("DELETE FROM PlanePlaneType");
            migrationBuilder.Sql("DELETE FROM Engines");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Engines', RESEED, 0)");
            migrationBuilder.Sql("DELETE FROM Planes");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Planes', RESEED, 0)");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airports_CurrentAirportId",
                table: "Planes");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentAirportId",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airports_CurrentAirportId",
                table: "Planes",
                column: "CurrentAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airports_CurrentAirportId",
                table: "Planes");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentAirportId",
                table: "Planes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airports_CurrentAirportId",
                table: "Planes",
                column: "CurrentAirportId",
                principalTable: "Airports",
                principalColumn: "Id");
        }
    }
}

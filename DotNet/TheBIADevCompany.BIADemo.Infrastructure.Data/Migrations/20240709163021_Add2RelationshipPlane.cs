using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add2RelationshipPlane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentAirportId",
                table: "Planes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlanePlaneType",
                columns: table => new
                {
                    PlaneId = table.Column<int>(type: "int", nullable: false),
                    PlaneTypeId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanePlaneType", x => new { x.PlaneId, x.PlaneTypeId });
                    table.ForeignKey(
                        name: "FK_PlanePlaneType_PlanesTypes_PlaneTypeId",
                        column: x => x.PlaneTypeId,
                        principalTable: "PlanesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanePlaneType_Planes_PlaneId",
                        column: x => x.PlaneId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Planes_CurrentAirportId",
                table: "Planes",
                column: "CurrentAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanePlaneType_PlaneTypeId",
                table: "PlanePlaneType",
                column: "PlaneTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airports_CurrentAirportId",
                table: "Planes",
                column: "CurrentAirportId",
                principalTable: "Airports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airports_CurrentAirportId",
                table: "Planes");

            migrationBuilder.DropTable(
                name: "PlanePlaneType");

            migrationBuilder.DropIndex(
                name: "IX_Planes_CurrentAirportId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "CurrentAirportId",
                table: "Planes");
        }
    }
}

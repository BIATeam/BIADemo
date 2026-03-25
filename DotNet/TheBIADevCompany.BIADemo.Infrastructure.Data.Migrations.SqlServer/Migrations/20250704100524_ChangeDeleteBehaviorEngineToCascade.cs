using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorEngineToCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines");

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines");

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Planes_PlaneId",
                table: "Engines",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id");
        }
    }
}

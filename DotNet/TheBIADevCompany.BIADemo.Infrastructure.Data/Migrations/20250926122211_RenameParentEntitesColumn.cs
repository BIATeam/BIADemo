using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameParentEntitesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentEntityId",
                table: "UsersAudit",
                newName: "LinkedEntities");

            migrationBuilder.RenameColumn(
                name: "ParentEntityId",
                table: "PlaneAudit",
                newName: "LinkedEntities");

            migrationBuilder.RenameColumn(
                name: "ParentEntityId",
                table: "PlaneAirportAudit",
                newName: "LinkedEntities");

            migrationBuilder.RenameColumn(
                name: "ParentEntityId",
                table: "EngineAudit",
                newName: "LinkedEntities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkedEntities",
                table: "UsersAudit",
                newName: "ParentEntityId");

            migrationBuilder.RenameColumn(
                name: "LinkedEntities",
                table: "PlaneAudit",
                newName: "ParentEntityId");

            migrationBuilder.RenameColumn(
                name: "LinkedEntities",
                table: "PlaneAirportAudit",
                newName: "ParentEntityId");

            migrationBuilder.RenameColumn(
                name: "LinkedEntities",
                table: "EngineAudit",
                newName: "ParentEntityId");
        }
    }
}

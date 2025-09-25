using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddParentPropertyIdOnAuditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentEntityId",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentEntityId",
                table: "AirportsAudit",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentEntityId",
                table: "UsersAudit");

            migrationBuilder.DropColumn(
                name: "ParentEntityId",
                table: "AirportsAudit");
        }
    }
}

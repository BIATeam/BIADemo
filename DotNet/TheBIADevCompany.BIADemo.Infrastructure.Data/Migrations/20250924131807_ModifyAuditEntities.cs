using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAuditEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAudit",
                table: "UsersAudit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AirportsAudit",
                table: "AirportsAudit");

            migrationBuilder.DropColumn(
                name: "AuditId",
                table: "UsersAudit");

            migrationBuilder.DropColumn(
                name: "AuditId",
                table: "AirportsAudit");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UsersAudit",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "UsersAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AirportsAudit",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "AirportsAudit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAudit",
                table: "UsersAudit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AirportsAudit",
                table: "AirportsAudit",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAudit",
                table: "UsersAudit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AirportsAudit",
                table: "AirportsAudit");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "UsersAudit");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "AirportsAudit");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UsersAudit",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AuditId",
                table: "UsersAudit",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AirportsAudit",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AuditId",
                table: "AirportsAudit",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAudit",
                table: "UsersAudit",
                column: "AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AirportsAudit",
                table: "AirportsAudit",
                column: "AuditId");
        }
    }
}

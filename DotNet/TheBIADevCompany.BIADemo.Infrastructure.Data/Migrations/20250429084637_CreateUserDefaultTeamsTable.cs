using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserDefaultTeamsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDefaultTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDefaultTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDefaultTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDefaultTeams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultTeams_TeamId",
                table: "UserDefaultTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultTeams_UserId_TeamId",
                table: "UserDefaultTeams",
                columns: new[] { "UserId", "TeamId" },
                unique: true);

            migrationBuilder.Sql(@"
                INSERT INTO UserDefaultTeams (UserId, TeamId)
                SELECT UserId, TeamId
                FROM Members
                WHERE IsDefault = 1;
            ");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"
                INSERT INTO Members (UserId, TeamId, IsDefault)
                SELECT UserId, TeamId, 1
                FROM UserDefaultTeams;
            ");

            migrationBuilder.DropTable(
                name: "UserDefaultTeams");
        }
    }
}

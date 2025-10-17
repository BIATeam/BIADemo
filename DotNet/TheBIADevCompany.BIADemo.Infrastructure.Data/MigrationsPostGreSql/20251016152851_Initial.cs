using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.MigrationsPostGreSql
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    City = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirportsAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    City = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AuditAction = table.Column<string>(type: "text", nullable: true),
                    AuditChanges = table.Column<string>(type: "text", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportsAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Table = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "text", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AuditAction = table.Column<string>(type: "text", nullable: true),
                    AuditChanges = table.Column<string>(type: "text", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistCache",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(449)", maxLength: 449, nullable: false),
                    Value = table.Column<byte[]>(type: "bytea", nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(type: "bigint", nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SN = table.Column<string>(type: "text", nullable: true),
                    Family = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CertificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastSyncDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersAudit",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Domain = table.Column<string>(type: "text", nullable: false, defaultValue: "--"),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AuditAction = table.Column<string>(type: "text", nullable: true),
                    AuditChanges = table.Column<string>(type: "text", nullable: true),
                    AuditUserLogin = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAudit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Preference = table.Column<string>(type: "text", nullable: false),
                    ViewType = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypeTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTypeTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTypeTranslations_NotificationTypes_Notification~",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTranslations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleTeamTypes",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "integer", nullable: false),
                    TeamTypesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTeamTypes", x => new { x.RolesId, x.TeamTypesId });
                    table.ForeignKey(
                        name: "FK_RoleTeamTypes_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTeamTypes_TeamTypes_TeamTypesId",
                        column: x => x.TeamTypesId,
                        principalTable: "TeamTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TeamTypeId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_TeamTypes_TeamTypeId",
                        column: x => x.TeamTypeId,
                        principalTable: "TeamTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    Read = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: true),
                    JData = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViewUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ViewId = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewUser", x => new { x.UserId, x.ViewId });
                    table.ForeignKey(
                        name: "FK_ViewUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViewUser_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AircraftMaintenanceCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftMaintenanceCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AircraftMaintenanceCompanies_Teams_Id",
                        column: x => x.Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sites_Teams_Id",
                        column: x => x.Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDefaultTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ViewTeam",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    ViewId = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewTeam", x => new { x.TeamId, x.ViewId });
                    table.ForeignKey(
                        name: "FK_ViewTeam_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViewTeam_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificationId = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTeam_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTeam_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotificationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    NotificationId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTranslations_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationUser",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUser", x => new { x.UserId, x.NotificationId });
                    table.ForeignKey(
                        name: "FK_NotificationUser_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    AircraftMaintenanceCompanyId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: true),
                    FirstOperation = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastOperation = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NextOperation = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MaxTravelDuration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    MaxOperationDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    OperationCount = table.Column<int>(type: "integer", nullable: false),
                    IncidentCount = table.Column<int>(type: "integer", nullable: true),
                    TotalOperationDuration = table.Column<double>(type: "double precision", nullable: false),
                    AverageOperationDuration = table.Column<double>(type: "double precision", nullable: true),
                    TotalTravelDuration = table.Column<float>(type: "real", nullable: false),
                    AverageTravelDuration = table.Column<float>(type: "real", nullable: true),
                    TotalOperationCost = table.Column<decimal>(type: "Money", nullable: false),
                    AverageOperationCost = table.Column<decimal>(type: "Money", nullable: true),
                    CurrentAirportId = table.Column<int>(type: "integer", nullable: false),
                    CurrentCountryId = table.Column<int>(type: "integer", nullable: true),
                    IsFixed = table.Column<bool>(type: "boolean", nullable: false),
                    FixedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceTeams_AircraftMaintenanceCompanies_AircraftMaint~",
                        column: x => x.AircraftMaintenanceCompanyId,
                        principalTable: "AircraftMaintenanceCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceTeams_Airports_CurrentAirportId",
                        column: x => x.CurrentAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceTeams_Countries_CurrentCountryId",
                        column: x => x.CurrentCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceTeams_Teams_Id",
                        column: x => x.Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRole",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRole", x => new { x.MemberId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_MemberRole_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    AircraftMaintenanceCompanyId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    IsFixed = table.Column<bool>(type: "boolean", nullable: false),
                    FixedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceContract_AircraftMaintenanceCompanies_AircraftMa~",
                        column: x => x.AircraftMaintenanceCompanyId,
                        principalTable: "AircraftMaintenanceCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceContract_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msn = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsMaintenance = table.Column<bool>(type: "boolean", nullable: true),
                    FirstFlightDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastFlightDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "date", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "date", nullable: false),
                    SyncTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    SyncFlightDataTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    MotorsCount = table.Column<int>(type: "integer", nullable: true),
                    TotalFlightHours = table.Column<double>(type: "double precision", nullable: false),
                    Probability = table.Column<double>(type: "double precision", nullable: true),
                    FuelCapacity = table.Column<float>(type: "real", nullable: false),
                    FuelLevel = table.Column<float>(type: "real", nullable: true),
                    OriginalPrice = table.Column<decimal>(type: "Money", nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "Money", nullable: true),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    PlaneTypeId = table.Column<int>(type: "integer", nullable: true),
                    CurrentAirportId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    IsFixed = table.Column<bool>(type: "boolean", nullable: false),
                    FixedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planes_Airports_CurrentAirportId",
                        column: x => x.CurrentAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Planes_PlanesTypes_PlaneTypeId",
                        column: x => x.PlaneTypeId,
                        principalTable: "PlanesTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Planes_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTeamRole",
                columns: table => new
                {
                    NotificationTeamId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTeamRole", x => new { x.NotificationTeamId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_NotificationTeamRole_NotificationTeam_NotificationTeamId",
                        column: x => x.NotificationTeamId,
                        principalTable: "NotificationTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTeamRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceTeamAirport",
                columns: table => new
                {
                    MaintenanceTeamId = table.Column<int>(type: "integer", nullable: false),
                    AirportId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
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
                    MaintenanceTeamId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Engines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "date", nullable: false),
                    ExchangeDate = table.Column<DateTime>(type: "date", nullable: true),
                    SyncTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IgnitionTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Power = table.Column<int>(type: "integer", nullable: true),
                    NoiseLevel = table.Column<int>(type: "integer", nullable: false),
                    FlightHours = table.Column<double>(type: "double precision", nullable: false),
                    AverageFlightHours = table.Column<double>(type: "double precision", nullable: true),
                    FuelConsumption = table.Column<float>(type: "real", nullable: false),
                    AverageFuelConsumption = table.Column<float>(type: "real", nullable: true),
                    OriginalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    PlaneId = table.Column<int>(type: "integer", nullable: false),
                    IsToBeMaintained = table.Column<bool>(type: "boolean", nullable: false),
                    IsHybrid = table.Column<bool>(type: "boolean", nullable: true),
                    PrincipalPartId = table.Column<int>(type: "integer", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    IsFixed = table.Column<bool>(type: "boolean", nullable: false),
                    FixedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Engines_Parts_PrincipalPartId",
                        column: x => x.PrincipalPartId,
                        principalTable: "Parts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Engines_Planes_PlaneId",
                        column: x => x.PlaneId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceContractPlane",
                columns: table => new
                {
                    MaintenanceContractId = table.Column<int>(type: "integer", nullable: false),
                    PlaneId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceContractPlane", x => new { x.MaintenanceContractId, x.PlaneId });
                    table.ForeignKey(
                        name: "FK_MaintenanceContractPlane_MaintenanceContract_MaintenanceCon~",
                        column: x => x.MaintenanceContractId,
                        principalTable: "MaintenanceContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceContractPlane_Planes_PlaneId",
                        column: x => x.PlaneId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAirport",
                columns: table => new
                {
                    PlaneId = table.Column<int>(type: "integer", nullable: false),
                    AirportId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirport", x => new { x.AirportId, x.PlaneId });
                    table.ForeignKey(
                        name: "FK_PlaneAirport_Airports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaneAirport_Planes_PlaneId",
                        column: x => x.PlaneId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanePlaneType",
                columns: table => new
                {
                    PlaneId = table.Column<int>(type: "integer", nullable: false),
                    PlaneTypeId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "EnginePart",
                columns: table => new
                {
                    EngineId = table.Column<int>(type: "integer", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnginePart", x => new { x.EngineId, x.PartId });
                    table.ForeignKey(
                        name: "FK_EnginePart_Engines_EngineId",
                        column: x => x.EngineId,
                        principalTable: "Engines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnginePart_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
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

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "EN", "English" },
                    { 2, "FR", "Français" },
                    { 3, "ES", "Española" },
                    { 4, "DE", "Deutsch" }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 1, "task", "Task" },
                    { 2, "info", "Info" },
                    { 3, "success", "Success" },
                    { 4, "warn", "Warn" },
                    { 5, "error", "Error" }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Family", "Price", "SN" },
                values: new object[,]
                {
                    { 1, "N.A", 499.99m, "P0001" },
                    { 2, "N.A", 250.99m, "P0002" },
                    { 3, "N.A", 100.99m, "P0003" },
                    { 4, "N.A", 25.99m, "P0004" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "Label" },
                values: new object[,]
                {
                    { 1, "Site_Admin", "Airline administrator" },
                    { 2, "Pilot", "Pilot" },
                    { 3, "AircraftMaintenanceCompany_Admin", "AircraftMaintenanceCompany administrator" },
                    { 4, "MaintenanceTeam_Admin", "MaintenanceTeam administrator" },
                    { 101, "Supervisor", "Supervisor" },
                    { 102, "Expert", "Expert" },
                    { 201, "Team_Leader", "Team leader" },
                    { 202, "Operator", "Operator" },
                    { 10001, "Admin", "Administrator" },
                    { 10002, "Back_Admin", "Background task administrator" },
                    { 10003, "Back_Read_Only", "Visualization of background tasks" }
                });

            migrationBuilder.InsertData(
                table: "TeamTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Root" },
                    { 2, "Site" },
                    { 3, "AircraftMaintenanceCompany" },
                    { 4, "MaintenanceTeam" }
                });

            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Description", "Name", "Preference", "TableId", "ViewType" },
                values: new object[] { -1, null, "default", "{\"first\":0,\"rows\":10,\"sortField\":\"createdDate\",\"sortOrder\":-1,\"columnOrder\":[\"titleTranslated\",\"descriptionTranslated\",\"type\",\"read\",\"createdDate\",\"createdBy\"],\"selection\":[],\"filters\":{}}", "notificationsGrid", 0 });

            migrationBuilder.InsertData(
                table: "NotificationTypeTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "NotificationTypeId" },
                values: new object[,]
                {
                    { 101, "Tâche", 2, 1 },
                    { 102, "Tarea", 3, 1 },
                    { 103, "Aufgabe", 4, 1 },
                    { 201, "Information", 2, 2 },
                    { 202, "Información", 3, 2 },
                    { 203, "Information", 4, 2 },
                    { 301, "Succès", 2, 3 },
                    { 302, "Éxito", 3, 3 },
                    { 303, "Erfolg", 4, 3 },
                    { 401, "Avertissement", 2, 4 },
                    { 402, "Advertencia", 3, 4 },
                    { 403, "Erwärmen", 4, 4 },
                    { 501, "Erreur", 2, 5 },
                    { 502, "Culpa", 3, 5 },
                    { 503, "Fehler", 4, 5 }
                });

            migrationBuilder.InsertData(
                table: "RoleTeamTypes",
                columns: new[] { "RolesId", "TeamTypesId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 101, 3 },
                    { 102, 3 },
                    { 102, 4 },
                    { 201, 4 },
                    { 202, 4 },
                    { 10001, 1 },
                    { 10002, 1 },
                    { 10003, 1 }
                });

            migrationBuilder.InsertData(
                table: "RoleTranslations",
                columns: new[] { "Id", "Label", "LanguageId", "RoleId" },
                values: new object[,]
                {
                    { 101, "Administrateur de la compagnie", 2, 1 },
                    { 102, "Administrador de la aerolínea", 3, 1 },
                    { 103, "Fluglinienadministrator", 4, 1 },
                    { 201, "Pilote", 2, 2 },
                    { 202, "Piloto", 3, 2 },
                    { 203, "Pilot", 4, 2 },
                    { 10101, "Superviseur", 2, 101 },
                    { 10102, "Supervisor", 3, 101 },
                    { 10103, "Supervisor", 4, 101 },
                    { 10201, "Expert", 2, 102 },
                    { 10202, "Experto", 3, 102 },
                    { 10203, "Experte", 4, 102 },
                    { 20101, "Chef d'equipe", 2, 201 },
                    { 20102, "Jefe de equipo", 3, 201 },
                    { 20103, "Teamleiter", 4, 201 },
                    { 20201, "Operateur", 2, 202 },
                    { 20202, "Operador", 3, 202 },
                    { 20203, "Operator", 4, 202 },
                    { 1000101, "Administrateur", 2, 10001 },
                    { 1000102, "Administrador", 3, 10001 },
                    { 1000103, "Administrator", 4, 10001 },
                    { 1000201, "Administrateur des tâches en arrière-plan", 2, 10002 },
                    { 1000202, "Administrador de tareas en segundo plano", 3, 10002 },
                    { 1000203, "Administrator für Hintergrundaufgaben", 4, 10002 },
                    { 1000301, "Visualisation des tâches en arrière-plan", 2, 10003 },
                    { 1000302, "Visualización de tareas en segundo plano", 3, 10003 },
                    { 1000303, "Visualisierung von Hintergrundaufgaben", 4, 10003 }
                });

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                table: "DistCache",
                column: "ExpiresAtTime");

            migrationBuilder.CreateIndex(
                name: "IX_EnginePart_PartId",
                table: "EnginePart",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Engines_PlaneId",
                table: "Engines",
                column: "PlaneId");

            migrationBuilder.CreateIndex(
                name: "IX_Engines_PrincipalPartId",
                table: "Engines",
                column: "PrincipalPartId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceContract_AircraftMaintenanceCompanyId",
                table: "MaintenanceContract",
                column: "AircraftMaintenanceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceContract_SiteId",
                table: "MaintenanceContract",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceContractPlane_PlaneId",
                table: "MaintenanceContractPlane",
                column: "PlaneId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeamAirport_MaintenanceTeamId",
                table: "MaintenanceTeamAirport",
                column: "MaintenanceTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeamCountry_MaintenanceTeamId",
                table: "MaintenanceTeamCountry",
                column: "MaintenanceTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeams_AircraftMaintenanceCompanyId",
                table: "MaintenanceTeams",
                column: "AircraftMaintenanceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeams_CurrentAirportId",
                table: "MaintenanceTeams",
                column: "CurrentAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTeams_CurrentCountryId",
                table: "MaintenanceTeams",
                column: "CurrentCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRole_RoleId",
                table: "MemberRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_TeamId_UserId",
                table: "Members",
                columns: new[] { "TeamId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedById",
                table: "Notifications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TypeId",
                table: "Notifications",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTeam_NotificationId",
                table: "NotificationTeam",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTeam_TeamId",
                table: "NotificationTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTeamRole_RoleId",
                table: "NotificationTeamRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTranslations_LanguageId",
                table: "NotificationTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTranslations_NotificationId_LanguageId",
                table: "NotificationTranslations",
                columns: new[] { "NotificationId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslations_LanguageId",
                table: "NotificationTypeTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslations_NotificationTypeId_LanguageId",
                table: "NotificationTypeTranslations",
                columns: new[] { "NotificationTypeId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUser_NotificationId",
                table: "NotificationUser",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaneAirport_PlaneId",
                table: "PlaneAirport",
                column: "PlaneId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanePlaneType_PlaneTypeId",
                table: "PlanePlaneType",
                column: "PlaneTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_CurrentAirportId",
                table: "Planes",
                column: "CurrentAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_PlaneTypeId",
                table: "Planes",
                column: "PlaneTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_SiteId",
                table: "Planes",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTeamTypes_TeamTypesId",
                table: "RoleTeamTypes",
                column: "TeamTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTranslations_LanguageId",
                table: "RoleTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTranslations_RoleId_LanguageId",
                table: "RoleTranslations",
                columns: new[] { "RoleId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamTypeId",
                table: "Teams",
                column: "TeamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultTeams_TeamId",
                table: "UserDefaultTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultTeams_UserId_TeamId",
                table: "UserDefaultTeams",
                columns: new[] { "UserId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersId",
                table: "UserRoles",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViewTeam_ViewId",
                table: "ViewTeam",
                column: "ViewId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewUser_ViewId",
                table: "ViewUser",
                column: "ViewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirportsAudit");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "DistCache");

            migrationBuilder.DropTable(
                name: "EnginePart");

            migrationBuilder.DropTable(
                name: "MaintenanceContractPlane");

            migrationBuilder.DropTable(
                name: "MaintenanceTeamAirport");

            migrationBuilder.DropTable(
                name: "MaintenanceTeamCountry");

            migrationBuilder.DropTable(
                name: "MemberRole");

            migrationBuilder.DropTable(
                name: "NotificationTeamRole");

            migrationBuilder.DropTable(
                name: "NotificationTranslations");

            migrationBuilder.DropTable(
                name: "NotificationTypeTranslations");

            migrationBuilder.DropTable(
                name: "NotificationUser");

            migrationBuilder.DropTable(
                name: "PlaneAirport");

            migrationBuilder.DropTable(
                name: "PlanePlaneType");

            migrationBuilder.DropTable(
                name: "RoleTeamTypes");

            migrationBuilder.DropTable(
                name: "RoleTranslations");

            migrationBuilder.DropTable(
                name: "UserDefaultTeams");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UsersAudit");

            migrationBuilder.DropTable(
                name: "ViewTeam");

            migrationBuilder.DropTable(
                name: "ViewUser");

            migrationBuilder.DropTable(
                name: "Engines");

            migrationBuilder.DropTable(
                name: "MaintenanceContract");

            migrationBuilder.DropTable(
                name: "MaintenanceTeams");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "NotificationTeam");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Planes");

            migrationBuilder.DropTable(
                name: "AircraftMaintenanceCompanies");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "PlanesTypes");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "TeamTypes");
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class RemoveSidAndDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login_Domain",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Sid",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "--");

            migrationBuilder.AddColumn<string>(
                name: "Sid",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "--");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login_Domain",
                table: "Users",
                columns: new[] { "Login", "Domain" },
                unique: true);
        }
    }
}

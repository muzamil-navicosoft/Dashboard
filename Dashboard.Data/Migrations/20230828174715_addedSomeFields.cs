using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedSomeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Ticket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReolved",
                table: "Ticket",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "DateReolved",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "Ticket");
        }
    }
}

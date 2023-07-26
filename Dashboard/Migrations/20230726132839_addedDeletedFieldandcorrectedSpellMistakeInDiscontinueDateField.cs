using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class addedDeletedFieldandcorrectedSpellMistakeInDiscontinueDateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountneDate",
                table: "ClientForm",
                newName: "DiscontinueDate");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "ClientForm",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "ClientForm");

            migrationBuilder.RenameColumn(
                name: "DiscontinueDate",
                table: "ClientForm",
                newName: "DiscountneDate");
        }
    }
}

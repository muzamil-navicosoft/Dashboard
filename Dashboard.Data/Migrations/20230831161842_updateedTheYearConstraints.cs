using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateedTheYearConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "BillingInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Year",
                table: "BillingInfo",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}

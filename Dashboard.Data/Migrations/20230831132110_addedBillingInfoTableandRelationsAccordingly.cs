using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedBillingInfoTableandRelationsAccordingly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OneTimeBill",
                table: "ClientForm",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    ClientFormId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingInfo_ClientForm_ClientFormId",
                        column: x => x.ClientFormId,
                        principalTable: "ClientForm",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillingInfo_ClientFormId",
                table: "BillingInfo",
                column: "ClientFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingInfo");

            migrationBuilder.DropColumn(
                name: "OneTimeBill",
                table: "ClientForm");
        }
    }
}

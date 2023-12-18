using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedthebillinginfomodel_with_detialsField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Resolution",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BillingInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e5ddeb3c-3891-404e-a247-d8ee0fa04550", "AQAAAAIAAYagAAAAENUoAJ2cNgKrYIPJYO9P7lngnw96J6AEErHAMWClHyRKiTHTzmVg3WZQeOujCPysLw==", "82bf1891-71b6-4063-809f-f575aa7eb77a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BillingInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Resolution",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "087a2c8b-fd6b-4640-bf75-8bbef2ec8c24", "AQAAAAIAAYagAAAAEEmOoAXmlU4e55CHG8OiVK5VzPUDtBrJIdly6R4/qaWTPYlIngeJC/n1xuZmFXmPTA==", "83b9aec5-d299-40fe-91cc-b40ab7562a57" });
        }
    }
}

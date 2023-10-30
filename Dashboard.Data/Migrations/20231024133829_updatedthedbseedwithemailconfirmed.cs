using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedthedbseedwithemailconfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "087a2c8b-fd6b-4640-bf75-8bbef2ec8c24", true, "AQAAAAIAAYagAAAAEEmOoAXmlU4e55CHG8OiVK5VzPUDtBrJIdly6R4/qaWTPYlIngeJC/n1xuZmFXmPTA==", "83b9aec5-d299-40fe-91cc-b40ab7562a57" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26cf5a47-8d6b-42e4-85d6-f6be0b89b9d2", false, "AQAAAAIAAYagAAAAEFpchHTHsQV43tHABUnKK/KqXuGxbznzvIJm6gw5Cira1BR1DzbWUhUrZKdnnpQ7CA==", "5216dc77-a054-4e34-9349-56bdb55d429b" });
        }
    }
}

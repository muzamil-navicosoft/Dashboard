using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26cf5a47-8d6b-42e4-85d6-f6be0b89b9d2", "AQAAAAIAAYagAAAAEFpchHTHsQV43tHABUnKK/KqXuGxbznzvIJm6gw5Cira1BR1DzbWUhUrZKdnnpQ7CA==", "5216dc77-a054-4e34-9349-56bdb55d429b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e5a41704-f917-49f7-9a67-8b2bd0fc43f8", "AQAAAAIAAYagAAAAEG/zXbRDczckD+4QbUS88jbwv9osl/4RaEBRRtiPxgQLlIZnxW0PT9Jp0LF2UhXX1g==", "7f6073ea-dddb-40a7-905d-7e97e2a04446" });
        }
    }
}

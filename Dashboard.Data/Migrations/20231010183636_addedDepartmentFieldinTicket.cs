using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedDepartmentFieldinTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e5a41704-f917-49f7-9a67-8b2bd0fc43f8", "AQAAAAIAAYagAAAAEG/zXbRDczckD+4QbUS88jbwv9osl/4RaEBRRtiPxgQLlIZnxW0PT9Jp0LF2UhXX1g==", "7f6073ea-dddb-40a7-905d-7e97e2a04446" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Ticket");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f06625f3-5cf0-487e-be5c-c76242561bf8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aa7e8803-247d-4ef3-bcb3-ad4a7b9341b8", "AQAAAAIAAYagAAAAEP0hc9cqWZH9Au73/gZBhhYAuOARatMLZikVcpZ+bJ3LGaGXf3I+ARPARQc5RGpeWw==", "63e64f70-5bbc-4d90-a223-92add7cbf3ec" });
        }
    }
}

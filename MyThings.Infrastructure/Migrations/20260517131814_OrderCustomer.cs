using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustomerId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CustomerId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId1",
                table: "Order",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustomerId1",
                table: "Order",
                column: "CustomerId1",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}

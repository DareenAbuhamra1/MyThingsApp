using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderTweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_User_CustomerId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Orders",
                newName: "StartEstimation");

            migrationBuilder.RenameColumn(
                name: "OrderCode",
                table: "Orders",
                newName: "DeliveryLocationId");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Orders",
                newName: "EndEstimation");

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "OrderLineOption",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderLineOption",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderLineOption",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryLocationId",
                table: "Orders",
                column: "DeliveryLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Location_DeliveryLocationId",
                table: "Orders",
                column: "DeliveryLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Location_DeliveryLocationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryLocationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "OrderLineOption");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderLineOption");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderLineOption");

            migrationBuilder.RenameColumn(
                name: "StartEstimation",
                table: "Orders",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "EndEstimation",
                table: "Orders",
                newName: "End");

            migrationBuilder.RenameColumn(
                name: "DeliveryLocationId",
                table: "Orders",
                newName: "OrderCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_User_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLine_Orders_OrderId",
                table: "OrderLine");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customer_CustomerId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryRule_DeliveryRuleId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Domain_DomainId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Driver_DriverId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Location_DeliveryLocationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Partner_PartnerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PartnerId",
                table: "Order",
                newName: "IX_Order_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DriverId",
                table: "Order",
                newName: "IX_Order_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DomainId",
                table: "Order",
                newName: "IX_Order_DomainId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryRuleId",
                table: "Order",
                newName: "IX_Order_DeliveryRuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryLocationId",
                table: "Order",
                newName: "IX_Order_DeliveryLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId1",
                table: "Order",
                newName: "IX_Order_CustomerId1");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustomerId1",
                table: "Order",
                column: "CustomerId1",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DeliveryRule_DeliveryRuleId",
                table: "Order",
                column: "DeliveryRuleId",
                principalTable: "DeliveryRule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Domain_DomainId",
                table: "Order",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Driver_DriverId",
                table: "Order",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Location_DeliveryLocationId",
                table: "Order",
                column: "DeliveryLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Partner_PartnerId",
                table: "Order",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLine_Order_OrderId",
                table: "OrderLine",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustomerId1",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_DeliveryRule_DeliveryRuleId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Domain_DomainId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Driver_DriverId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Location_DeliveryLocationId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Partner_PartnerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLine_Order_OrderId",
                table: "OrderLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_PartnerId",
                table: "Orders",
                newName: "IX_Orders_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DriverId",
                table: "Orders",
                newName: "IX_Orders_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DomainId",
                table: "Orders",
                newName: "IX_Orders_DomainId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DeliveryRuleId",
                table: "Orders",
                newName: "IX_Orders_DeliveryRuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DeliveryLocationId",
                table: "Orders",
                newName: "IX_Orders_DeliveryLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId1",
                table: "Orders",
                newName: "IX_Orders_CustomerId1");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLine_Orders_OrderId",
                table: "OrderLine",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customer_CustomerId1",
                table: "Orders",
                column: "CustomerId1",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryRule_DeliveryRuleId",
                table: "Orders",
                column: "DeliveryRuleId",
                principalTable: "DeliveryRule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Domain_DomainId",
                table: "Orders",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Driver_DriverId",
                table: "Orders",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Location_DeliveryLocationId",
                table: "Orders",
                column: "DeliveryLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Partner_PartnerId",
                table: "Orders",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductOptionTweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOption_OrderLine_OrderLineId",
                table: "ProductOption");

            migrationBuilder.DropIndex(
                name: "IX_ProductOption_OrderLineId",
                table: "ProductOption");

            migrationBuilder.DropColumn(
                name: "OrderLineId",
                table: "ProductOption");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderLineId",
                table: "ProductOption",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOption_OrderLineId",
                table: "ProductOption",
                column: "OrderLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOption_OrderLine_OrderLineId",
                table: "ProductOption",
                column: "OrderLineId",
                principalTable: "OrderLine",
                principalColumn: "Id");
        }
    }
}

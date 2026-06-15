using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderLineRestriction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineOption_OrderLine_OrderLineId",
                table: "OrderLineOption");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderLineOption",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderLine",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineOption_OrderLine_OrderLineId",
                table: "OrderLineOption",
                column: "OrderLineId",
                principalTable: "OrderLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineOption_OrderLine_OrderLineId",
                table: "OrderLineOption");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderLineOption",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderLine",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineOption_OrderLine_OrderLineId",
                table: "OrderLineOption",
                column: "OrderLineId",
                principalTable: "OrderLine",
                principalColumn: "Id");
        }
    }
}

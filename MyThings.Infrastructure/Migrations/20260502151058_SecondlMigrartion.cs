using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecondlMigrartion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partner_Location_LocationId1",
                table: "Partner");

            migrationBuilder.DropIndex(
                name: "IX_Partner_LocationId",
                table: "Partner");

            migrationBuilder.DropIndex(
                name: "IX_Partner_LocationId1",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "Partner");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_LocationId",
                table: "Partner",
                column: "LocationId",
                unique: true,
                filter: "[LocationId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Partner_LocationId",
                table: "Partner");

            migrationBuilder.AddColumn<int>(
                name: "LocationId1",
                table: "Partner",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partner_LocationId",
                table: "Partner",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_LocationId1",
                table: "Partner",
                column: "LocationId1",
                unique: true,
                filter: "[LocationId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Partner_Location_LocationId1",
                table: "Partner",
                column: "LocationId1",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}

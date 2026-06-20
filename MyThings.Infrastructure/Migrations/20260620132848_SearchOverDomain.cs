using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SearchOverDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Partner",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Partner",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Partner",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreparingTime",
                table: "Partner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Partner",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Partner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Domain",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "PreparingTime",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Domain");
        }
    }
}

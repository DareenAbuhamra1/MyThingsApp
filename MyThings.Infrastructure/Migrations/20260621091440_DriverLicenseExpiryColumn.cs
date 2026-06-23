using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DriverLicenseExpiryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DriverLicenseExpiry",
                table: "Driver",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 6, 21, 9, 14, 39, 555, DateTimeKind.Utc).AddTicks(6850));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverLicenseExpiry",
                table: "Driver");
        }
    }
}

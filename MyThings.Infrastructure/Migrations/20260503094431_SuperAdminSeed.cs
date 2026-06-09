using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SuperAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuperAdmin",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Job",
                columns: new[] { "Id", "CanManageAccounts", "CanManageLogistics", "CanManageProducts", "CreatedAt", "DeletedAt", "Title", "UpdatedAt" },
                values: new object[] { 1, true, true, true, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "Super Admin", null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "DeletedAt", "Email", "FirstName", "Gender", "IsActive", "IsAdmin", "IsSuperAdmin", "LastLogin", "LastName", "Phone", "RefreshToken", "RefreshTokenExpiry", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2004, 4, 8), null, "superadmin@mythings.app", "Dareen", 2, true, true, true, null, "Abuhamra", "0790000000", null, null, null });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "Id", "Department", "EmployeeId", "JobId", "PasswordHash" },
                values: new object[] { 1, "Operations", "5000", 1, "$2a$11$KscYwNeQTX2GmzSJiHQBmu.WUcm7.m7FThl7i/sQUJdw7PGaz8ywO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsSuperAdmin",
                table: "User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAdminDetailsWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerStatusId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Search",
                table: "Customer",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Customer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "default");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "CustomerStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoundTextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsVideo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Alt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WHRatio = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerStatusTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerStatusId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStatusTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerStatusTranslation_CustomerStatus_CustomerStatusId",
                        column: x => x.CustomerStatusId,
                        principalTable: "CustomerStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerStatusTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypeTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypeTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTypeTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AvailabilityId",
                table: "Customer",
                column: "AvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerStatusId",
                table: "Customer",
                column: "CustomerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_LanguageId",
                table: "Customer",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MediaId",
                table: "Customer",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TenantId",
                table: "Customer",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TenantId_AvailabilityId_CustomerStatusId",
                table: "Customer",
                columns: new[] { "TenantId", "AvailabilityId", "CustomerStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStatusTranslation_CustomerStatusId_LanguageId",
                table: "CustomerStatusTranslation",
                columns: new[] { "CustomerStatusId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStatusTranslation_LanguageId",
                table: "CustomerStatusTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypeTranslation_CustomerTypeId_LanguageId",
                table: "CustomerTypeTranslation",
                columns: new[] { "CustomerTypeId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypeTranslation_LanguageId",
                table: "CustomerTypeTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_Code",
                table: "Language",
                column: "Code",
                unique: true);

            // Seed reference data BEFORE adding foreign keys
            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Name", "Code", "IsActive", "CreatedAt", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "English", "en", true, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, "Arabic", "ar", true, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false }
                });

            migrationBuilder.InsertData(
                table: "CustomerStatus",
                columns: new[] { "Id", "Name", "CreatedAt", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Active", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, "Inactive", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 3, "Suspended", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 4, "Banned", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 5, "Pending Verification", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), false }
                });

            // Update existing customers to have valid CustomerStatusId before adding foreign key constraint
            migrationBuilder.Sql("UPDATE [Customer] SET [CustomerStatusId] = 1 WHERE [CustomerStatusId] = 0 OR [CustomerStatusId] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerStatus_CustomerStatusId",
                table: "Customer",
                column: "CustomerStatusId",
                principalTable: "CustomerStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Language_LanguageId",
                table: "Customer",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Media_MediaId",
                table: "Customer",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerStatus_CustomerStatusId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Language_LanguageId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Media_MediaId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerStatusTranslation");

            migrationBuilder.DropTable(
                name: "CustomerTypeTranslation");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "CustomerStatus");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropIndex(
                name: "IX_Customer_AvailabilityId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CustomerStatusId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_LanguageId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_MediaId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_TenantId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_TenantId_AvailabilityId_CustomerStatusId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerStatusId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Search",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Customer");
        }
    }
}

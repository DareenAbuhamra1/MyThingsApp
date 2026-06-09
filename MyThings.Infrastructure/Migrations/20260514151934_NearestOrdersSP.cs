using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NearestOrdersSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.GetNearestOrders
                    @driverLat DECIMAL(8, 6),
                    @driverLon DECIMAL(9, 6),
                    @MaxDistanceKm FLOAT = 5,
                    @TopCount INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT TOP (@TopCount)
                        o.Id, 
                        o.SubTotal, 
                        o.TotalPayment, 
                        o.DeliveryFees, 
                        o.Status,
                        p.Id AS PartnerId,
                        p.Name AS PartnerName,
                        l.Country,
                        l.City,
                        l.Area,
                        l.Street,
                        l.Latitude,
                        l.Longitude,
                        calc.DistanceInKm
                    FROM [Order] o
                    INNER JOIN Partner p ON o.PartnerId = p.Id
                    INNER JOIN [Location] l ON p.LocationId = l.Id 
                    CROSS APPLY (
                        SELECT (6371 * ACOS(
                            SIN(RADIANS(l.Latitude)) * SIN(RADIANS(@driverLat)) + 
                            COS(RADIANS(l.Latitude)) * COS(RADIANS(@driverLat)) * COS(RADIANS(@driverLon - l.Longitude))
                        )) AS DistanceInKm
                    ) AS calc
                    WHERE 
                        o.Status = 3
                        AND o.DriverId IS NULL 
                        AND calc.DistanceInKm <= @MaxDistanceKm
                    ORDER BY calc.DistanceInKm;
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS dbo.GetNearestOrders");
        }
    }
}

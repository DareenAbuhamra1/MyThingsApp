using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyThings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GetNearestDriversSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.GetNearestDrivers
                        @OrderLat FLOAT,
                        @OrderLon FLOAT,
                        @MaxDistanceKm FLOAT = 5,
                        @TopCount INT = 10
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        SELECT TOP (@TopCount)
                            d.Id,
                            u.FirstName,
                            u.LastName,
                            u.Phone,
                            d.Latitude,
                            d.Longitude,
                            calc.DistanceInKm
                        FROM [Driver] d
                        INNER JOIN [User] u ON d.Id = u.Id
                        CROSS APPLY(
                        SELECT (6371 * ACOS(
                                SIN(RADIANS(d.Latitude)) * SIN(RADIANS(@OrderLat)) + 
                                COS(RADIANS(d.Latitude)) * COS(RADIANS(@OrderLat)) * 
                                COS(RADIANS(@OrderLon - d.Longitude))
                            )) AS DistanceInKm
                        ) AS calc
                        WHERE d.IsOnline = 1 
                            AND d.IsAssigned = 0
                            AND u.IsActive = 1
                            AND calc.DistanceInKm <= @MaxDistanceKm
                        
                        ORDER BY 
                            calc.DistanceInKm
                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS dbo.GetNearestDrivers");
        }
    }
}

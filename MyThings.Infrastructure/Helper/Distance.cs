namespace MyThings.Infrastructure.Helper;

public static class LocationHelper
{
    private const double EarthRadiusKm = 6371.0;

    public static double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        double rLat1 = ToRadians((double)lat1);
        double rLat2 = ToRadians((double)lat2);
        double dLat = ToRadians((double)(lat2 - lat1));
        double dLon = ToRadians((double)(lon2 - lon1));

        // Haversine formula
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(rLat1) * Math.Cos(rLat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
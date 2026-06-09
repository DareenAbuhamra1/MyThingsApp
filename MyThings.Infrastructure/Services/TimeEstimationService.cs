using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace MyThings.Infrastructure.Services;

public class TimeEstimationService : ITimeEstimationService
{
    public double EstimateDeliveryTime(PartnerCustomerLocDto locDto)
    {
        
        double Distance = LocationHelper.CalculateDistance(locDto.PartnerLat, locDto.PartnerLon
        ,locDto.CustomerLat, locDto.CustomerLon);

        return (double)Distance *2;
    }
    
    public async Task<TimeEstimateDto> EstimateDeliveryIntervalAsync(PartnerCustomerLocDto locDto)
    {
        double Distance = LocationHelper.CalculateDistance(locDto.PartnerLat, locDto.PartnerLon
        ,locDto.CustomerLat, locDto.CustomerLon);

        var minTime = Distance*2+20;
        var maxTime = Distance*2+30;

        return new TimeEstimateDto
        {
            StartEstimate = TimeOnly.FromDateTime(DateTime.UtcNow).AddMinutes(minTime) ,
            EndEstimate =  TimeOnly.FromDateTime(DateTime.UtcNow).AddMinutes(maxTime)
        };
    }
    
}
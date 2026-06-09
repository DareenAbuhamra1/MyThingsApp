using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace MyThings.Infrastructure.Services;

public class DeliveryFeeService : IDeliveryFeeService
{
    private readonly IReadUnitOfWork _readUnitOfWork;

    public DeliveryFeeService(IReadUnitOfWork readUnitOfWork)
    {
        _readUnitOfWork = readUnitOfWork;
    }

    public async Task<decimal> CalculateDeliveryFee(int partnerId, int deliveryLocId, int deliveryRuleId, decimal OrderTotal)
    {
       
        var deliveryRule = await _readUnitOfWork.DeliveryRules.GetByIdAsync(deliveryRuleId);
        
        
        if (deliveryRule == null)
        {
           throw new KeyNotFoundException("Delivery rule not found.");
        }
        
        if(deliveryRule.MinTotalForFreeDelivery <= OrderTotal)
        {
            return 0m;
        }
        
        var Partner = await _readUnitOfWork.Partners.GetByIdAsync(partnerId);
        var CustomerLocation = await _readUnitOfWork.Locations.GetByIdAsync(deliveryLocId);

        if (Partner == null || CustomerLocation == null)
        throw new KeyNotFoundException("Partner or Customer Location not found.");

        decimal lat1 = Partner.Location.Latitude;
        decimal lon1 = Partner.Location.Longitude;
        decimal lat2 = CustomerLocation.Latitude;
        decimal lon2 = CustomerLocation.Longitude;
        
        var Distance = LocationHelper.CalculateDistance(lat1, lon1,lat2,lon2) ;

        decimal DeliveryFee = deliveryRule.BaseFee + ((decimal)Distance * deliveryRule.PerKmFee);

        return  Math.Round(DeliveryFee, 3);
    }
}
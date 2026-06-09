using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;

public interface ITimeEstimationService
{
    double EstimateDeliveryTime(PartnerCustomerLocDto locDto);
    
    Task<TimeEstimateDto> EstimateDeliveryIntervalAsync(PartnerCustomerLocDto locDto);
}


namespace MyThings.Core.Interfaces;
public interface IDeliveryFeeService
{
    Task<decimal> CalculateDeliveryFee(int partnerId, int deliveryLocId, int deliveryRuleId,decimal OrderTotal);
}
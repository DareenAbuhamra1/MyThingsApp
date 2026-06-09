using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace MyThings.Infrastructure.Services;

public class LogisticService : ILogisiticService
{
    private readonly IUnitOfWork _unitOfWork;

    public LogisticService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    Task<Category?> ILogisiticService.CreateCategoriesAsync(CategoryCreationDto categoryCreation)
    {
        throw new NotImplementedException();
    }

    public async Task<DeliveryRule?> CreateDeliveryRuleAsync(DeliveryRuleDto deliveryRuleCreation)
    {
        var newRule = new DeliveryRule
        {
            City = deliveryRuleCreation.City,
            BaseFee = deliveryRuleCreation.BaseFee,
            PerKmFee = deliveryRuleCreation.PerKmFee,
            MinTotalForFreeDelivery = deliveryRuleCreation.MinTotalForFreeDelivery,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.DeliveryRules.AddAsync(newRule);
        var result = await _unitOfWork.CompleteAsync();

        if (result == 0 || newRule.Id <= 0)
        {
            return null;
        }
        return newRule;
    }

    
    
}

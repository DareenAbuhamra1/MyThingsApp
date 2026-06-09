using MyThings.Core.DTOs;
using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;

public interface ILogisiticService
{
    //Create Domains and Categories (Logistics)
    Task<Category?> CreateCategoriesAsync(CategoryCreationDto categoryCreation);
    Task<DeliveryRule?> CreateDeliveryRuleAsync(DeliveryRuleDto deliveryRuleCreation);
    
    //Manage Partners (Logistics)
    //Manage Orders (Logistics)
}
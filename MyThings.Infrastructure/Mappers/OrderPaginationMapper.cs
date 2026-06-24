using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrderPaginationMapper
{
    [MapProperty(nameof(Order.Id), nameof(OrderForPaginationDto.OrderId))]
    /*
    [MapperIgnoreSource(nameof(Order.CustomerId))]
    [MapperIgnoreSource(nameof(Order.Customer))]
    [MapperIgnoreSource(nameof(Order.Driver))]
    [MapperIgnoreSource(nameof(Order.OrderLines))]
    [MapperIgnoreSource(nameof(Order.SavingAmount))]
    [MapperIgnoreSource(nameof(Order.PaymentType))]
    [MapperIgnoreSource(nameof(Order.DriverId))]
    [MapperIgnoreSource(nameof(Order.PartnerId))]
    [MapperIgnoreSource(nameof(Order.DomainId))]
    [MapperIgnoreSource(nameof(Order.DeliveryRuleId))]
    [MapperIgnoreSource(nameof(Order.DeliveryLocationId))]
    [MapperIgnoreSource(nameof(Order.CreatedAt))]
    [MapperIgnoreSource(nameof(Order.UpdatedAt))]
    [MapperIgnoreSource(nameof(Order.DeletedAt))]
    [MapperIgnoreSource(nameof(Order.Partner))]
    [MapperIgnoreSource(nameof(Order.Domain))]
    [MapperIgnoreSource(nameof(Order.DeliveryRule))]
    [MapperIgnoreSource(nameof(Order.DeliveryLocation))]
    [MapperIgnoreSource(nameof(Order.IsDeleted))]
    */
    public partial OrderForPaginationDto Map(Order order);
}
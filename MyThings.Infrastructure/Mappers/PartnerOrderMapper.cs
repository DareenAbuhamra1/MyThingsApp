namespace MyThings.Infrastructure.Mappers;

using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;


[Mapper(UseDeepCloning = false,RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PartnerOrderMapper
{
    public partial PartnerOrderInfoDto Map(Order order);

    private partial PartnerOrderItem Map(OrderLine ol);

    private partial PartnerOrderItemOption Map(OrderLineOption olp);
}

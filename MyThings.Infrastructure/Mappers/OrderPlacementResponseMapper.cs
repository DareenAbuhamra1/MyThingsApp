using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, UseDeepCloning = false)]
public partial class OrderPlacementResponseMapper
{ 
    public partial OrderPlacementResponseDto Map(Order o);   
    private partial OrderLinePlacementDto Map(OrderLine ol);
    private partial OrderLineOptionPlacementDto Map(OrderLineOption olp);
    
}

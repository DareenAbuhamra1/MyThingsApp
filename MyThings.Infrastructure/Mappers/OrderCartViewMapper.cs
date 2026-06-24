using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, UseDeepCloning = false)]
public partial class OrderCartViewMapper{
    [MapProperty(nameof(Order.Id), nameof(OrderCartViewDto.OrderId))]
    [MapProperty(nameof(Order.TotalPayment), nameof(OrderCartViewDto.TotalPrice))]
    [MapProperty(nameof(Order.DeliveryLocation), nameof(OrderCartViewDto.DeliveryLocation))]
    [MapProperty(nameof(Order.Partner.Name), nameof(OrderCartViewDto.PartnerName))]
    [MapProperty(nameof(Order.DeliveryLocation), nameof(OrderCartViewDto.DeliveryLocation))]
    public partial OrderCartViewDto Map(Order o);
    private string MapDeliveryLocation(Location location)
        => location is null
        ? string.Empty
        : $"{location.Street}, {location.Area}, {location.City.ToString()}";  

    public partial OrderLineView Map(OrderLine ol);
    [MapProperty(nameof(OrderLineOption.Option), nameof(OrderLineOptionsView.ProductOption))]
    public partial OrderLineOptionsView Map(OrderLineOption olp);
}

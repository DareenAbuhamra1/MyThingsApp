using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class OrderCartDto
{
    public int OrderId {get;set;}
    public required int DeliveryLocationId {get;set;}
    public required OrderStatusEnum Status {get;set;}
    public required int CustomerId {get;set;}
    public required int PartnerId {get;set;}
    public required int DomainId {get;set;}
    public required OrderLineCart OrderLine {get;set;} = null!;
}
public class OrderLineCart
{
    public required int ProductId {get;set;}
    public required int Quantity {get;set;}
    public string Note {get;set;} = string.Empty;
    public List<OrderLineOptionsCart>? OrderLineOptions {get;set;}
}
public class OrderLineOptionsCart
{
    public int ProductOptionId {get;set;}
    public int Quantity {get;set;}
}
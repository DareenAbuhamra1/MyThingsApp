namespace MyThings.Core.DTOs;

public class OrderLineCartDto
{
    public int OrderId {get;set;}
    public int ProductId {get;set;}
    public int Quantity {get;set;}
    public List<OrderLineOptionsCartDto>? OrderLineOptions {get;set;}
}
public class OrderLineOptionsCartDto
{
    public int ProductOptionId {get;set;}
    public int Quantity {get;set;}
}
    
using MyThings.Core.Enums;

public class OrderCartViewDto
{
    public int OrderId { get; set; }
    public required string DeliveryLocation { get; set; }
    public required OrderStatusEnum Status { get; set; }
    public required int CustomerId { get; set; }
    public required int PartnerId { get; set; }
    public required string PartnerName { get; set; }
    public required decimal SubTotal { get; set; }
    public required decimal DeliveryFees { get; set; }
    public required decimal TotalPrice { get; set; }

    public List<OrderLineView> OrderLines { get; set; } = [];
}
public class OrderLineView
{
    public required int ProductId {get;set;}
    public required string ProductName {get;set;}
    public required int Quantity {get;set;}
    public List<OrderLineOptionsView> OrderLineOptions {get;set;} =[];
}
public class OrderLineOptionsView
{
    public int ProductOptionId {get;set;}
    public string ProductOption {get;set;} = null!;
    public int Quantity {get;set;}
}
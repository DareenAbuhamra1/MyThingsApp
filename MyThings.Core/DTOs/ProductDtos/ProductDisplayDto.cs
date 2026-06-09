namespace MyThings.Core.DTOs;


public class ProductDisplayDto
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; } = decimal.MinValue;
    public required int Stock { get; set; } = int.MinValue;
    public required string Description { get; set; } = string.Empty;
}
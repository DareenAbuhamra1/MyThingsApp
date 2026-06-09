namespace MyThings.Core.DTOs;

public class ProductOptionDisplayDto
{
    public int OptionGroupId { get; set; }
    public int ProductId {get;set;}
    public string Title { get; set; } = string.Empty;
    public bool IsRequired { get; set; } 
    public int MinSelection { get; set; }
    public int MaxSelection {get;set;}
    public List<ProductOption> Options { get; set; } = [];
    
}
public class ProductOption
{
    public int ProductOptionId {get;set;}
    public string Option {get;set;} = string.Empty;
    public decimal Price {get;set;}
}

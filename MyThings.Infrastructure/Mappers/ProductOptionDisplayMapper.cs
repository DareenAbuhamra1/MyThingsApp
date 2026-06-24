using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, UseDeepCloning = false)]
public partial class ProductOptionDisplayMapper
{
    [MapProperty(nameof(OptionGroup.Id), nameof(ProductOptionDisplayDto.OptionGroupId))]
    [MapProperty(nameof(OptionGroup.ProductId), nameof(ProductOptionDisplayDto.ProductId))]
    [MapProperty(nameof(OptionGroup.ProductOptions), nameof(ProductOptionDisplayDto.Options))]
    public partial ProductOptionDisplayDto Map(Core.Entities.OptionGroup og);
    [MapProperty(nameof(Core.Entities.ProductOption.Id), nameof(Core.DTOs.ProductOption.ProductOptionId))]
    public partial Core.DTOs.ProductOption Map(Core.Entities.ProductOption po);
}

/*
The member OptionGroupId on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
The member ProductId on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
The member Title on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
The member IsRequired on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
The member MinSelection on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
The member MaxSelection on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
The member Options on the mapping target type MyThings.Core.DTOs.ProductOptionDisplayDto was not found on the mapping source type MyThings.Core.Entities.Product(RMG012)
No members are mapped in the object mapping from MyThings.Core.Entities.Product to MyThings.Core.DTOs.ProductOptionDisplayDtoRMG066

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


 public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public bool IsRequired { get; set; }

    public int MinSelection { get; set; }

    public int MaxSelection { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual ICollection<ProductOption>? ProductOptions { get; set; } = [];

*/


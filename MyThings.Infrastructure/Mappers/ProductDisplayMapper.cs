using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]

public partial class ProductDisplayMapper
{
    [MapProperty(nameof(Product.Description), nameof(ProductDisplayDto.Description))]
    public partial ProductDisplayDto Map(Product p);

}

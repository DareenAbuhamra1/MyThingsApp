
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CutomerLocationMapper
{
    [MapProperty(nameof(Location.Id), nameof(CustomerLocationDto.LocationId))]
    [MapProperty(nameof(Location.Title), nameof(CustomerLocationDto.AddressTitle))]
    public partial CustomerLocationDto Map(Location l);
}

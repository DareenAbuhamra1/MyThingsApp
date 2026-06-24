using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LocationMapper
{
    [MapProperty(nameof(Location.Id), nameof(LocationDto.LocationId))]
    public partial LocationDto Map(Location l);
}
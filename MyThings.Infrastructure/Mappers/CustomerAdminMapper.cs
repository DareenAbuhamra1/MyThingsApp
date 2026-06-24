using MyThings.Core.DTOs.CustomerAdminDtos;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CustomerAdminMapper
{
    [MapProperty(nameof(Customer.Id), nameof(CustomerDetailsForAdminDto.Id))]
    [MapProperty(nameof(Customer.Language.Name), nameof(CustomerDetailsForAdminDto.LanguageName))]
    [MapProperty(nameof(Customer.LanguageId), nameof(CustomerDetailsForAdminDto.LanguageId))]
    [MapProperty(nameof(Customer.TypeId), nameof(CustomerDetailsForAdminDto.TypeId))]
    [MapProperty(nameof(Customer.CustomerStatusId), nameof(CustomerDetailsForAdminDto.CustomerStatusId))]
    [MapProperty(nameof(Customer.MediaId), nameof(CustomerDetailsForAdminDto.MediaId))]
    [MapProperty(nameof(Customer.Media), nameof(CustomerDetailsForAdminDto.Media))]
    public partial CustomerDetailsForAdminDto Map(Customer customer,string customerStatusName,string customerTypeName,string fullName );

    public partial MediaDetailDto Map(Media media);
}
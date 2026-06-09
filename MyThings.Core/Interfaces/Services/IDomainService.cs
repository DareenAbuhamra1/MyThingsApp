using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;

public interface IDomainService
{
    Task<DomainCreationDto?> CreateDomainAsync(DomainCreationDto domainCreation);
    Task<bool> AttachPartnerToDomainAsync(int partnerId, int domainId);
    Task<IReadOnlyList<DomainInfoDto>> GetAllDomainsAsync();
    
}
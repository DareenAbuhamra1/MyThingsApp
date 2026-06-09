using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace MyThings.Infrastructure.Services;


public class DomainService : IDomainService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadUnitOfWork _readUnitOfWork;

    public DomainService(IUnitOfWork unitOfWork,IReadUnitOfWork readUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _readUnitOfWork = readUnitOfWork;
    }

    public async Task<bool> AttachPartnerToDomainAsync(int partnerId, int domainId)
    {
        var partner = await _unitOfWork.Partners.GetByIdAsync(partnerId);

        if (partner == null)
        {
            return false;
        }
        var domain = await _unitOfWork.Domains.GetByIdAsync(domainId);

        if (domain == null)
        {
            return false;
        }

        var existingAssociation = await _unitOfWork.PartnerDomains.FindAsync(pd => pd.PartnerId == partnerId
        && pd.DomainId == domainId);

        // the Partner is already attached to the Domain, we can consider this as a successful operation without doing anything
        if (existingAssociation)
        {
            return true;
        }
        var newPartnerDomain = new PartnerDomain
        {
            PartnerId = partnerId,
            DomainId = domainId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.PartnerDomains.AddAsync(newPartnerDomain);
        var result = await _unitOfWork.CompleteAsync();

        if (result == 0)
        {
            return false;
        }
        return true;
    }

    public async Task<DomainCreationDto?> CreateDomainAsync(DomainCreationDto domainCreation)
    {
        var newDomain = new Domain
        {
            Name = domainCreation.Name,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.Domains.AddAsync(newDomain);
        var result = await _unitOfWork.CompleteAsync();
        if (result == 0 || newDomain.Id <= 0)
        {
            return null;
        }
        return new DomainCreationDto
        {
            Name = newDomain.Name,
        };
    }

    public async Task<IReadOnlyList<DomainInfoDto>> GetAllDomainsAsync()
    {
        var domains = await _readUnitOfWork.Domains.GetAllAsync();
        var result = new List<DomainInfoDto>();

        foreach(var d in domains)
        {
            result.Add(new DomainInfoDto
            {
                Id = d.Id,
                Name = d.Name,
            });
        }
        return result;
    }
}
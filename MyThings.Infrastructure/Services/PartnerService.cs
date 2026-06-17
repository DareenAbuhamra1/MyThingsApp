using Microsoft.EntityFrameworkCore;
using Mythings.Core.Interaces.Services;
using MyThings.Core.Dto;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;

namespace MyThings.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly IPartnerReadRepository _partnerReadRepository;

    public PartnerService(IPartnerReadRepository partnerReadRepository)
    {
        _partnerReadRepository = partnerReadRepository;
    }
    public async Task<IReadOnlyList<PartnerListDto>> GetPartnerListAsync()
    {
        var partnerListQuery = _partnerReadRepository.GetPartnersList();

        var partnerList = await partnerListQuery
        .OrderByDescending(p => p.CreatedAt)
        .Select(
            p => new PartnerListDto
            {
                Id = p.Id,
                Name = p.Name,
                Area = p.Location.Area
            }
        )   
        .ToListAsync();

        return partnerList;
    }
}
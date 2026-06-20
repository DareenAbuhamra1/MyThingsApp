using Microsoft.EntityFrameworkCore;
using Mythings.Core.Interaces.Services;
using MyThings.Core.Dto;
using MyThings.Core.DTOs;
using MyThings.Core.DTOs.SPSearch;
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

    public async Task<PageResponse<PartnerListDto>> SearchOverDomain(SearchPartnersQueryDto query)
    {
        var searchQuery = _partnerReadRepository
            .SearchPartners(query.SearchTerm, query.DomainId, (double)query.Latitude, (double)query.Longitude);

        var totalCount = await searchQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        var partners = await searchQuery
            .OrderBy(p => p.Order)
            .ThenBy(p => p.Distance)
            .Skip((query.PageNumber -1 )*query.PageSize)
            .Take(query.PageSize)
            .Select(p => new PartnerListDto
            {
                Id = p.Partner.Id,
                Name  = p.Partner.Name,
                Area = p.Partner.Location.Area,
                DescriptionAr = p.Partner.DescriptionAr,
                DescriptionEn = p.Partner.DescriptionEn,
                Rating = p.Partner.Rating,
                RatingCount = p.Partner.RatingCount,
                Latitude = p.Partner.Location.Latitude,
                Longitude = p.Partner.Location.Longitude,
                Distance = p.Distance,
            }).ToListAsync();
    
        return new PageResponse<PartnerListDto>
        {
            Data = partners,
            TotalPages = totalPages,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            Page = query.PageNumber,
        };
    }
}

using MyThings.Core.Dto;
using MyThings.Core.DTOs;
using MyThings.Core.DTOs.SPSearch;


namespace Mythings.Core.Interaces.Services;

public interface IPartnerService
{
   Task<IReadOnlyList<PartnerListDto>> GetPartnerListAsync();
   //Task<IReadOnlyList<PartnerListDto>> SearchOverDomain(SearchPartnersQueryDto query);
   //Task<IReadOnlyList<PartnerListDto>> SearchPartnerName(string SearchTerm);
   //Task<IReadOnlyList<PartnerListDto>> SearchCategory(string SearchTerm);
   Task<PageResponse<PartnerListDto>> SearchOverDomain(SearchPartnersQueryDto query);
}
using MyThings.Core.Dto;
using MyThings.Core.Entities;
using MyThings.Core.Wrappers;

namespace Mythings.Core.Interaces.Services;

public interface IPartnerService
{
   Task<IReadOnlyList<PartnerListDto>> GetPartnerListAsync();
}
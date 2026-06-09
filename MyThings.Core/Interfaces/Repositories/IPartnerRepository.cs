using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace Mythings.Core.Interaces.Repositories;

public interface IPartnerRepository :IGenericRepository<Partner>
{
    Task<Partner?> GetWorkingHoursAsync(int PartnerId);
    
}
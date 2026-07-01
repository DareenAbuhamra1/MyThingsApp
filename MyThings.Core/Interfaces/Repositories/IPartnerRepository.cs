using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace MyThings.Core.Interfaces;

public interface IPartnerRepository :IGenericRepository<Partner>
{
    Task<Partner?> GetWorkingHoursAsync(int PartnerId);
    
}
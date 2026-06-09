using Microsoft.EntityFrameworkCore;
using Mythings.Core.Interaces.Repositories;
using MyThings.Core.Entities;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
{
    public PartnerRepository(WriteDbContext context) : base(context) { }
    public async Task<Partner?> GetWorkingHoursAsync(int PartnerId)
    {
        return await _context.Partners
            .Include(w => w.WorkingHours)
            .FirstOrDefaultAsync(p => p.Id == PartnerId); 
    }
}
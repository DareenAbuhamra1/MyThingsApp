using MyThings.Core.Entities;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class AdminRepository : GenericRepository<Admin>, IAdminRepository
{
    public AdminRepository(WriteDbContext context) : base(context)
    {
    }

    public async Task<Admin?> GetByEmailAsync(string email)
    {
        return await _context.Set<Admin>().FirstOrDefaultAsync(
            x => EF.Property<string>(x,"Email") == email
        );
    }
}
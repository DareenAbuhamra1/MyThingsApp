using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MyThings.Infrastructure.Repositories;


public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    protected readonly ReadDbContext _context;


    public ReadOnlyRepository(ReadDbContext context)
    {
        _context = context;

    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(
            x => EF.Property<int>(x, "Id") ==id
        );
    }

}
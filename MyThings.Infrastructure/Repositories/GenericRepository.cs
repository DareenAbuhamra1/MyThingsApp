using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class{

    protected readonly WriteDbContext _context;

    public GenericRepository(WriteDbContext context)
    {
        _context = context;
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        // FindAsync() checks memory first before going to the Db
        //return await _context.Set<T>().FindAsync(id);

        return await _context.Set<T>().FirstOrDefaultAsync(
            x => EF.Property<int>(x, "Id") == id
        );
    }
    public async Task<T?> GetByPhoneAsync(string phone)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(
            x => EF.Property<string>(x, "Phone") == phone
        );
    }
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }
    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
    public void Delete(T entity)
    {
        if(entity is ISoftDeletable softDeletable)
        {
            softDeletable.IsDeleted = true;
            softDeletable.DeletedAt = DateTime.UtcNow;

            _context.Set<T>().Update(entity);
        }
        else
        {
            _context.Set<T>().Remove(entity);
        }
    }
    public async Task<bool> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var result = await _context.Set<T>().FirstOrDefaultAsync(predicate);
        return result != null;
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class WorkingHoursRepository : IWorkingHoursRepository
{
    private readonly WriteDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    public WorkingHoursRepository(WriteDbContext context,IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        
    }

    public async Task<bool> AddWorkingHoursAsync(int PartnerId, IEnumerable<WorkingHour> newShifts)
    {
        var Partner = await _context.Partners
            .Include(p=> p.WorkingHours)
            .FirstOrDefaultAsync(p => p.Id == PartnerId);
        
        if(Partner == null) return false;

        _context.RemoveRange(Partner.WorkingHours);

        foreach(var w in newShifts)
        {
            Partner.WorkingHours.Add(w);
        }
        var affectedRows = await _context.SaveChangesAsync();
        return affectedRows > 0;
            
    }

    public async Task<Partner?> GetWorkingHoursAsync(int PartnerId)
    {
        return await _context.Partners
            .Include(w => w.WorkingHours)
            .FirstOrDefaultAsync(p => p.Id == PartnerId);
    }
    
}
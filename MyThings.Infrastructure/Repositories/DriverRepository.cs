using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly WriteDbContext _context;
    private readonly IReadOnlyRepository<Driver> _readOnlyRepo;
    public DriverRepository(WriteDbContext context,IReadOnlyRepository<Driver> readOnlyRepo)
    {
        _context = context;
        _readOnlyRepo = readOnlyRepo;
    }

    
}
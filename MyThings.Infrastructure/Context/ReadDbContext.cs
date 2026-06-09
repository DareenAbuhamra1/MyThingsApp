using Microsoft.EntityFrameworkCore;
using MyThings.Core.Entities;

namespace MyThings.Infrastructure.Context;

public class ReadDbContext : BaseDbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options){}
}

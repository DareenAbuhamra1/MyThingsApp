
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

public interface IAdminRepository : IGenericRepository<Admin>
{
    Task<Admin?> GetByEmailAsync(string email);
}
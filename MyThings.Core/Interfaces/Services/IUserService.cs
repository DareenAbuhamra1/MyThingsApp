using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByPhoneAsync(string phone);

    Task<bool> IsUserActive(int id);
    Task<Admin?> GetByEmailAsync(string email);
    // more functions
}

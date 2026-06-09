using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace MyThings.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Admin?> GetByEmailAsync(string email)
    {
        return await _unitOfWork.Admins.GetByEmailAsync(email);
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task<User?> GetByPhoneAsync(string phone)
    {
        return await _unitOfWork.Users.GetByPhoneAsync(phone);
    }
    public Task<bool> IsUserActive(int id)
    {
        throw new NotImplementedException();
    }
}
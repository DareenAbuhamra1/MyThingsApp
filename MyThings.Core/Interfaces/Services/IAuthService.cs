using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces;

public interface IAuthService
{
    // Steps 1 & 2 remain the same (Generic for everyone)
    
    Task<int> RequestOtpAsync(string phone); 
    Task<AuthResultDto> VerifyOtpAsync(VerifyOtpRequestDto verifyOtpRequest);

    // Step 3: Branched Registration
    Task<AuthResponseDto> RegisterCustomerAsync(RegisterRequestDto request); // Base DTO is enough for customers
    Task<AuthResponseDto> RegisterDriverAsync(DriverRegisterDto request);
    Task<AuthResponseDto> CreatePartnerAsync(PartnerRegisterDto request); // Called by Admin

    // Step 4 & 5: Admin and Security
    Task<AuthResponseDto> LoginAdminAsync(AdminLoginDto dto); 
    Task<bool> ResetPasswordAsync(string email, string newPassword);
    Task<AuthResponseDto> RefreshTokenAsync(string expiredToken, string refreshToken);

    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<ServiceResponse<bool>> RevokeSessionAsync(int userId);
}
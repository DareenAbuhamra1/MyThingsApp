using System.Data.Common;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;

namespace MyThings.Auth.AuthServices;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMemoryCache _cache;
    private DateTime Expiry => DateTime.UtcNow.AddHours(
        double.Parse(_configuration["Jwt:ExpiryHours"] ?? "2"));

    public AuthService(
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IUserService userService,
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IMemoryCache cache)
    {
        _configuration = configuration;
        _logger = logger;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _cache = cache;
    }

    public async Task<AuthResponseDto> LoginAdminAsync(AdminLoginDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Email and Password are required."
                };
            }
            var user = await _userService.GetByEmailAsync(dto.Email);

            if (user is null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }
            if (user.Role is not (RoleEnum.Admin or RoleEnum.SuperAdmin))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Unauthorized access."
                };
            }
            bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }
            if(user.IsActive == false)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    IsActive = false,
                    Message = "Your account is not active."
                };
            }
            var token = _tokenService.GenerateJwtToken(user, user.Role);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            user.LastLogin = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
            
            return new AuthResponseDto
            {
                IsRegistered = true,
                IsActive = true,
                IsSuccess = true,
                Token = token,
                RefreshToken = refreshToken,
                Expiry = Expiry,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = user.Role,
                UserId = user.Id,
                Message = "Login successful."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin login");
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "An internal error occurred during login."
            };
        }
    }

    public Task<AuthResponseDto> RefreshTokenAsync(string expiredToken, string refreshToken)
    {
        return _tokenService.RefreshTokenAsync(expiredToken, refreshToken);
    }

    public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterRequestDto request)
    {
        var user = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email,
            Role = RoleEnum.Customer,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            WalletBalance = 0.00m
        };
    
        await _unitOfWork.Customers.AddAsync(user);
        await _unitOfWork.CompleteAsync();
        
        int userId = user.Id; // Get the generated ID after saving to the database

        if (userId > 0)
        {
            var token = _tokenService.GenerateJwtToken(user, user.Role);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto
            {
                IsRegistered = true,
                IsActive = true,
                IsSuccess = true,
                Token = token,
                RefreshToken = refreshToken,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = user.Role,
                UserId = userId,
                Message = "Customer registered successfully."
            };
        }
        return new AuthResponseDto
        {
            IsRegistered = false,
            IsActive = false,
            IsSuccess = false,
            Message = "Customer registration failed. Please try again."
        };
    }

    public async Task<AuthResponseDto> RegisterDriverAsync(DriverRegisterDto request)
    {
        var user = new Driver
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Role = RoleEnum.Driver,
            IsActive = false, // Drivers require admin approval
            CreatedAt = DateTime.UtcNow,
            VehicleLicense = request.VehicleLicense,
            DriverLicense = request.DriverLicense,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
        };

        await _unitOfWork.Drivers.AddAsync(user);
        var result = await _unitOfWork.CompleteAsync();

        if(result == 0 || user.Id <= 0)
        {
            return new AuthResponseDto
            {
                IsRegistered = false,
                IsActive = false,
                IsSuccess = false,
                Message = "Driver registration failed. Please try again."
            };
        }
        return new AuthResponseDto
        {
            IsRegistered = true,
            IsActive = false, // Inform the driver that their account is pending approval
            IsSuccess = true,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role,
            UserId = user.Id,
            Message = "Driver registered successfully. Your account is pending approval by an administrator."
        };
    }

    public Task<int> RequestOtpAsync(string phone)
    {
        if(!Regex.IsMatch(phone, @"^07[5789]\d{7}$"))
        {
            return Task.FromResult(-1);
        }

        Random random = new Random();
        int otp = random.Next(1000, 9999);

        _cache.Set(phone,otp,TimeSpan.FromMinutes(2));
        Console.WriteLine($"OTP for {phone}: {otp}"); // Simulate sending OTP by logging to console

        return Task.FromResult(otp);
    }
    public async Task<AuthResultDto> VerifyOtpAsync(VerifyOtpRequestDto verifyOtpRequest)
    {
        if(!_cache.TryGetValue(verifyOtpRequest.Phone, out int cachedOtp))
        {
            return new AuthResultDto
            {
                IsSuccess = false,
                Message = "OTP has expired. Please request a new one."
            };
        }
        //recheck the parsing logic might throw an exception try parse instead?
        if(!int.TryParse(verifyOtpRequest.OtpCode, out int otpCode) || otpCode != cachedOtp)
        {
            return new AuthResultDto
            {
                IsSuccess = false,
                Message = "Invalid OTP code. Please try again."
            };
        }
        _cache.Remove(verifyOtpRequest.Phone);

       var user = await _unitOfWork.Users.GetByPhoneAsync(verifyOtpRequest.Phone);
        
        if(user is null)
        {
            return new AuthResultDto
            {
                IsSuccess = true,
                IsRegistered = false, 
                VerifiedPhone = verifyOtpRequest.Phone,
                Message = "New User to register.",
                Role = verifyOtpRequest.Role,
            };
        }
        if(user.IsActive == false)
        {
            return new AuthResultDto
            {
                IsSuccess = false,
                IsRegistered = true, 
                Message = "Your account is suspended or waiting for approval. Please contact support."
            };
        }

        var token = _tokenService.GenerateJwtToken(user, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();
        
        return new AuthResultDto
        {
            IsSuccess = true,
            IsRegistered = true,
            Token = token,
            RefreshToken = refreshToken,
            Expiry = Expiry,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role,
            UserId = user.Id,
            Message = "Login successful."
        };
    }

    public Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        throw new NotImplementedException();
    }


    public Task<AuthResponseDto> CreatePartnerAsync(PartnerRegisterDto request)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userService.GetByPhoneAsync(dto.Phone);
        
        if (user is null)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                IsRegistered = false,
                Message = "User not found."
            };
        }
        if(user.IsActive == false)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                IsActive = false,
                Message = "Your account is not active. Please contact support."
            };
        }
        var token = _tokenService.GenerateJwtToken(user, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);
        user.LastLogin = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();  
        
        return new AuthResponseDto
        {
            IsRegistered = true,
            IsActive = true,
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken,
            Expiry = Expiry,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role,
            UserId = user.Id,
            Message = "Login successful."
        };
    }

    public async Task<ServiceResponse<bool>> RevokeSessionAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("Revocation failed: User ID {UserId} does not exist.", userId);
            return ServiceResponse<bool>.Failure("The user is not found",404);
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }
}

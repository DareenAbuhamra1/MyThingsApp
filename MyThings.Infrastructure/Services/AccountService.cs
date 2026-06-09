using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;

namespace MyThings.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDriverService _driverService;
    private readonly IAuditService _auditService;
    private readonly IUserAccessor _userAccessor;
    public AccountService(IUnitOfWork unitOfWork, IDriverService driverService, IAuditService auditService, IUserAccessor userAccessor)
    {
        _unitOfWork = unitOfWork;
        _driverService = driverService;
        _auditService = auditService;
        _userAccessor = userAccessor;
    }
    //rewrite the method
    public async Task<ServiceResponse<bool>> ActivateUserAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return ServiceResponse<bool>.Failure("The user is not found", 404);

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;

        var adminId = _userAccessor.GetCurrentUserId();

        await _auditService.LogActionAsync(adminId, "Activate User", "User", user.Id);
       
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<Driver>> ApproveDriverAsync(int driverId, bool active)
    {
        var Driver = await _driverService.ActivateDriverAsync(driverId, active);
        if (Driver == null) return ServiceResponse<Driver>.Failure("The driver is not found", 404);
        return ServiceResponse<Driver>.Ok(Driver);
    }
    //needs refactoring for Password Hash 
    public async Task<ServiceResponse<AdminInfoDto>> CreateAdminAsync(AdminInfoDto dto)
    {
        var admin = new Admin
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Role = dto.Role,
            IsActive = dto.IsActive,
            EmployeeId = dto.EmployeeId,
            JobId = dto.JobId,
            Department = dto.Department,
            PasswordHash = dto.PasswordHash,
        };

        await _unitOfWork.Admins.AddAsync(admin);
        await _unitOfWork.CompleteAsync();

        var adminResponse = new AdminInfoDto
        {
            Id = admin.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Role = dto.Role,
            IsActive = dto.IsActive,
            EmployeeId = dto.EmployeeId,
            JobId = dto.JobId,
            Department = dto.Department,
            PasswordHash = dto.PasswordHash,
        };

        return ServiceResponse<AdminInfoDto>.Ok(adminResponse);
    }

    public async Task<ServiceResponse<PartnerRegisterResponseDto>> CreatePartnerAsync(PartnerRegisterDto partnerInfo)
    {
        var partnerLocation = new Location
        {
            Title = $"{partnerInfo.Name} {partnerInfo.Area}",
            Country = partnerInfo.Country,
            City = partnerInfo.City,
            Area = partnerInfo.Area,
            Street = partnerInfo.Street,
            Latitude = partnerInfo.Latitude,
            Longitude = partnerInfo.Longitude,
            IsDefault = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Locations.AddAsync(partnerLocation);
        await _unitOfWork.CompleteAsync();

        var newPartner = new Partner
        {
            FirstName = partnerInfo.FirstName,
            LastName = partnerInfo.LastName,
            Email = partnerInfo.Email,
            Phone = partnerInfo.Phone,
            Gender = partnerInfo.Gender,
            Role = RoleEnum.Partner,
            DateOfBirth = partnerInfo.DateOfBirth,
            Name = partnerInfo.Name,
            RegistrationNo = partnerInfo.RegistrationNo,
            CommissionRate = partnerInfo.CommissionRate,
            ParentStoreId = partnerInfo.ParentStoreId,
            LocationId = partnerLocation.Id,
            DeliveryRuleId = partnerInfo.DeliveryRuleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Partners.AddAsync(newPartner);
        var result = await _unitOfWork.CompleteAsync();

        Console.WriteLine($"CreatePartnerAsync: result={result}, newPartnerId={newPartner.Id}");
        if (result == 0)
        {
            return ServiceResponse<PartnerRegisterResponseDto>.Failure("Creating partner failed", 500);
        }
        var partnerDto = new PartnerRegisterResponseDto
        {
            Id = newPartner.Id,
            Name = newPartner.Name,
            RegistrationNo = newPartner.RegistrationNo,
            CommissionRate = newPartner.CommissionRate,
            ParentStoreId = newPartner.ParentStoreId,
            Location = new PartnerLocationDto
            {
                Id = partnerLocation.Id,
                Title = partnerLocation.Title,
                Country = partnerLocation.Country.ToString(),
                City = partnerLocation.City.ToString(),
                Area = partnerLocation.Area,
                Street = partnerLocation.Street,
                Latitude = partnerLocation.Latitude,
                Longitude = partnerLocation.Longitude,
                IsDefault = partnerLocation.IsDefault
            },
            IsActive = newPartner.IsActive
        };
        return ServiceResponse<PartnerRegisterResponseDto>.Ok(partnerDto);
    }

    public async Task<ServiceResponse<bool>> DeactivateUserAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return ServiceResponse<bool>.Failure("The user is not found", 404);

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        var adminId = _userAccessor.GetCurrentUserId();

        await _auditService.LogActionAsync(adminId, "Deactivate User", "User", user.Id);
       
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public Task<bool> DeleteAdminAsync(int adminId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserRoleAsync(int userId, RoleEnum role)
    {
        throw new NotImplementedException();
    }
}

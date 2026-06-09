using MyThings.Core.Entities;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces;

public interface IAccountService
{

    Task<ServiceResponse<Driver>> ApproveDriverAsync(int driverId, bool active);

    Task<ServiceResponse<PartnerRegisterResponseDto>> CreatePartnerAsync(PartnerRegisterDto partnerInfo);

    Task<ServiceResponse<AdminInfoDto>> CreateAdminAsync(AdminInfoDto adminInfoDto);

    // Using 'int' or 'Guid' for consistency with your other IDs 
    Task<bool> DeleteAdminAsync(int adminId);
    Task<ServiceResponse<bool>> DeactivateUserAsync(int userId);
    Task<ServiceResponse<bool>> ActivateUserAsync(int userId);
    Task<bool> UpdateUserRoleAsync(int userId, RoleEnum role);

}
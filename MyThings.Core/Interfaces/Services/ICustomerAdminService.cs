using MyThings.Core.DTOs.CustomerAdminDtos;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces.Services;

public interface ICustomerAdminService
{
    Task<ServiceResponse<CustomerAdminResponseDto>> GetCustomersDetailsForAdminAsync(CustomerAdminFilterDto filter);
}

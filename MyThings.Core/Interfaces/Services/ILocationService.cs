using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;


public interface ILocationService
{
    Task<CustomerLocationDto?> GetCustomerDefaultLocation(int customerId);
    Task<LocationDto> CreateDefaultLocation(CustomerLocationDto locationDto);
}
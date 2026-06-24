using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.Entities;
using MyThings.Infrastructure.Mappers;

namespace MyThings.Infrastructure.Services;

public class LocationService : ILocationService
{
    private readonly ReadDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CutomerLocationMapper _customerLocationMapper;
    private readonly LocationMapper _locationMapper;
    public LocationService(ReadDbContext context,IUnitOfWork unitOfWork, CutomerLocationMapper customerLocationMapper, LocationMapper locationMapper)
    {
        _context =context;
        _unitOfWork = unitOfWork;
        _customerLocationMapper = customerLocationMapper;
        _locationMapper = locationMapper;
    }

    public async Task<CustomerLocationDto?> GetCustomerDefaultLocation(int customerId)
    {
        var CustomerLocDto = await _context.Locations
            .Where(l => l.CustomerId == customerId && l.IsDefault == true)
            .Select(l => _customerLocationMapper.Map(l))
            .FirstOrDefaultAsync();

        return CustomerLocDto;
    }

    public async Task<LocationDto> CreateDefaultLocation(CustomerLocationDto locationDto)
    {
        var DefaultCustomerLocation = new Location
            {
                Title = locationDto.AddressTitle,
                Country = locationDto.Country,
                City = locationDto.City,
                Area = locationDto.Area,
                Street = locationDto.Street,
                BuildingNo = locationDto.BuildingNo,
                ApartmentNo = locationDto.ApartmentNo,
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude,
                IsDefault = true,
                CustomerId = locationDto.CustomerId,
                CreatedAt = DateTime.UtcNow, 
            };
        
        await _unitOfWork.Locations.AddAsync(DefaultCustomerLocation);
        await _unitOfWork.CompleteAsync();
        
        return _locationMapper.Map(DefaultCustomerLocation);
    }
}

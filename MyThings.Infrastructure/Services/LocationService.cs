using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.Entities;
using MyThings.Infrastructure.Repositories;

namespace MyThings.Infrastructure.Services;

public class LocationService : ILocationService
{
    private readonly ReadDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    public LocationService(ReadDbContext context,IUnitOfWork unitOfWork)
    {
        _context =context;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerLocationDto?> GetCustomerDefaultLocation(int customerId)
    {
        var CustomerLocDto = await _context.Locations
            .Where(l => l.CustomerId == customerId && l.IsDefault == true)
            .Select(l => new CustomerLocationDto
            {
                CustomerId = customerId,
                LocationId = l.Id,
                Country = l.Country,
                City = l.City,
                Area = l.Area,
                Street = l.Street,
                BuildingNo = l.BuildingNo??"",
                ApartmentNo = l.ApartmentNo??"",
                Latitude = l.Latitude,
                Longitude = l.Longitude
            })
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
        
        return new LocationDto
        {
            LocationId = DefaultCustomerLocation.Id,
            CustomerId = locationDto.CustomerId,
            Title = DefaultCustomerLocation.Title,
            Country = DefaultCustomerLocation.Country.ToString(),
            City = DefaultCustomerLocation.City.ToString(),
            Area = DefaultCustomerLocation.Area,
            Street = DefaultCustomerLocation.Street,
            BuildingNo = DefaultCustomerLocation.BuildingNo??"",
            ApartmentNo = DefaultCustomerLocation.ApartmentNo??"",
            Latitude = DefaultCustomerLocation.Latitude,
            Longitude = DefaultCustomerLocation.Longitude,
            IsDefault = DefaultCustomerLocation.IsDefault,
        };
    }
}
using MyThings.Core.DTOs.CustomerAdminDtos;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Core.Interfaces.Services;
using MyThings.Core.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace MyThings.Infrastructure.Services;

public class CustomerAdminService : ICustomerAdminService
{
    private readonly IReadUnitOfWork _readUnitOfWork;

    public CustomerAdminService(IReadUnitOfWork readUnitOfWork)
    {
        _readUnitOfWork = readUnitOfWork;
    }

    public async Task<ServiceResponse<CustomerAdminResponseDto>> GetCustomersDetailsForAdminAsync(CustomerAdminFilterDto filter)
    {
        try
        {
            var statusIds = string.IsNullOrWhiteSpace(filter.CustomerStatuses)
                ? new List<int>()
                : filter.CustomerStatuses.Split(',')
                    .Select(s => s.Trim())
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .ToList();

            var countryIds = string.IsNullOrWhiteSpace(filter.SessionCountries)
                ? new List<int>()
                : filter.SessionCountries.Split(',')
                    .Select(s => s.Trim())
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .ToList();

            var cityIds = string.IsNullOrWhiteSpace(filter.SessionCities)
                ? new List<int>()
                : filter.SessionCities.Split(',')
                    .Select(s => s.Trim())
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .ToList();

            var query = _readUnitOfWork.Customers.GetQueryable()
                .Where(c => c.AvailabilityId == filter.AvailabilityType)
                .Where(c => c.TenantId == filter.TenantId)
                .Where(c => c.LanguageId == filter.LanguageId)
                .Where(c => filter.CustomerId == null || c.Id == filter.CustomerId);

            if (statusIds.Any())
            {
                query = query.Where(c => statusIds.Contains(c.CustomerStatusId));
            }

            if (countryIds.Any())
            {
                query = query.Where(c => c.CountryId.HasValue && countryIds.Contains(c.CountryId.Value));
            }

            if (cityIds.Any())
            {
                query = query.Where(c => c.CityId.HasValue && cityIds.Contains(c.CityId.Value));
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(c => 
                                       c.FirstName.ToLower().Contains(searchTerm) ||
                                       c.LastName.ToLower().Contains(searchTerm) ||
                                       (c.Email != null && c.Email.ToLower().Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .Include(c => c.Language)
                .Include(c => c.CustomerStatus)
                    .ThenInclude(cs => cs!.Translations)
                        .ThenInclude(cst => cst.Language)
                .Include(c => c.Media)
                .OrderBy(c => c.Id)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToListAsync();

            var customerDtos = customers.Select(c => MapToCustomerDetailsDto(c, filter.LanguageId)).ToList();

            var response = new CustomerAdminResponseDto
            {
                Customers = customerDtos,
                TotalCount = totalCount,
                PageNumber = (filter.Skip / filter.Take) + 1,
                PageSize = filter.Take
            };

            return ServiceResponse<CustomerAdminResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            return ServiceResponse<CustomerAdminResponseDto>.Failure(
                $"Error retrieving customer details: {ex.Message}", 500);
        }
    }

    private CustomerDetailsForAdminDto MapToCustomerDetailsDto(Customer customer, int languageId)
    {
        // Get translated status name for the requested language
        var statusTranslation = customer.CustomerStatus?.Translations
            .FirstOrDefault(t => t.LanguageId == languageId);
        
        var statusName = statusTranslation?.Name ?? customer.CustomerStatus?.Name ?? "Unknown";

        // Construct FullName
        var fullName = customer.TypeId == 7 // Guest type
            ? $"{customer.Email} {customer.FirstName} {customer.LastName}".Trim()
            : $"{customer.FirstName} {customer.LastName}".Trim();

        return new CustomerDetailsForAdminDto
        {
            Id = customer.Id,
            FullName = fullName,
            LanguageId = customer.LanguageId,
            LanguageName = customer.Language?.Name,
            TypeId = customer.TypeId,
            CustomerTypeName = GetCustomerTypeName(customer.TypeId),
            CustomerStatusId = customer.CustomerStatusId,
            CustomerStatusName = statusName,
            MediaId = customer.MediaId,
            Media = customer.Media != null ? MapToMediaDetailDto(customer.Media) : null
        };
    }

    private MediaDetailDto MapToMediaDetailDto(Media media)
    {
        return new MediaDetailDto
        {
            Id = media.Id,
            Color = media.Color,
            TextColor = media.TextColor,
            IsVideo = media.IsVideo,
            DisplayOrder = media.DisplayOrder,
            Name = media.Name,
            Alt = media.Alt,
            RoundTextColor = media.RoundTextColor,
            WHRatio = media.WHRatio,
            ImageUrl = media.ImageUrl
        };
    }

    private string GetCustomerTypeName(int typeId)
    {
        return typeId switch
        {
            1 => "Regular",
            2 => "Premium",
            7 => "Guest",
            _ => "Unknown"
        };
    }
}

namespace MyThings.Core.DTOs.CustomerAdminDtos;

public class CustomerAdminFilterDto
{
    public int AvailabilityType { get; set; }
    public string? CustomerStatuses { get; set; }
    public string? SessionCountries { get; set; }
    public string? SessionCities { get; set; }
    public string? Search { get; set; }
    public int? CustomerId { get; set; }
    public string TenantId { get; set; } = "default";
    public int LanguageId { get; set; } = 1;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}

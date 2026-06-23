namespace MyThings.Core.DTOs.CustomerAdminDtos;

public class CustomerAdminResponseDto
{
    public List<CustomerDetailsForAdminDto> Customers { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}

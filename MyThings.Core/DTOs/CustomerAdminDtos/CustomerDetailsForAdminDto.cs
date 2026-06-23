namespace MyThings.Core.DTOs.CustomerAdminDtos;

public class CustomerDetailsForAdminDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int LanguageId { get; set; }
    public string? LanguageName { get; set; }
    public int TypeId { get; set; }
    public string? CustomerTypeName { get; set; }
    public int CustomerStatusId { get; set; }
    public string? CustomerStatusName { get; set; }
    public int? MediaId { get; set; }
    public MediaDetailDto? Media { get; set; }
}

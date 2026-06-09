namespace MyThings.Core.DTOs;

public class PartnerRegisterResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string RegistrationNo { get; set; } = null!;
    public decimal CommissionRate { get; set; }
    public int? ParentStoreId { get; set; }
    public PartnerLocationDto Location { get; set; } = null!;
    public bool IsActive { get; set; }
}
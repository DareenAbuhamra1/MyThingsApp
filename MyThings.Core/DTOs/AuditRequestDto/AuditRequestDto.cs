namespace MyThings.Core.DTOs;

public class AuditRequestDto
{
    public required int AdminId {get;set;}
    public required string Action { get; set; } = null!; // e.g., "UpdateFee", "ApprovePartner"
    public required string EntityName { get; set; } = null!; // e.g., "Store", "Driver"
    public required int EntityId { get; set; } // The ID of the affected store/driver
}
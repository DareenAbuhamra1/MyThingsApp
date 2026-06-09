namespace MyThings.Core.DTOs;

public class StoreDisplayDto
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required bool IsAvailable {get;set;}
}
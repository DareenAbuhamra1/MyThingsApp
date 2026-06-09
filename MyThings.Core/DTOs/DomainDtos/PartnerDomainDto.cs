using System.ComponentModel.DataAnnotations;

namespace MyThings.Core.DTOs;

public class PartnerDomainDto
{
    [Required]
    public required int PartnerId { get; set; }
    [Required]
    public required int DomainId { get; set; }
}
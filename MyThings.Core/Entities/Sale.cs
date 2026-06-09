using MyThings.Core.Entities;

namespace Mythings.Core.Entities;

public class Sale : BaseEntity
{
    public required int ProductId {get;set;}
    public required string Type {get;set;}
    public required decimal Amount {get;set;}
    
}
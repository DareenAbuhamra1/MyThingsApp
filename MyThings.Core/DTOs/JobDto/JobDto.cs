namespace MyThings.Core.DTOs;

public class JobDto
{
    public int? Id {get;set;}
    public string Title {get;set;} = null!;
    public bool CanManageAccounts {get;set;}
    public bool CanManageLogistics {get;set;}
    public bool CanManageProducts {get;set;}
}
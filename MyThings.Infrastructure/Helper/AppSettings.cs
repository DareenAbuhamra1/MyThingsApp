namespace MyThings.Infrastructure.Helper;

public class AppSettings
{
    public string Key {get;set;} = null!;
    public string Issuer {get;set;} = null!;
    public string Audience {get;set;} =null!;
    public string[] AllowedRoles { get; set; } = [];
}

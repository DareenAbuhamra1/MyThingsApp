namespace MyThings.Core.Interfaces;

public interface IUserAccessor
{
    int GetCurrentUserId();
    string GetCurrentUserRole();
}
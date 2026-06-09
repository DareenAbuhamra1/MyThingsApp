namespace MyThings.Core.Interfaces;
public interface IAuditService {
    Task LogActionAsync(int adminId, string action, string entityName, int entityId);
}
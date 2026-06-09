using Microsoft.Extensions.Logging;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Repositories;

namespace MyThings.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly ILogger<AuditService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AuditService(ILogger<AuditService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task LogActionAsync(int adminId, string action, string entityName, int entityId)
    {
        var logDetails = new AuditLog
        {
            AdminId = adminId,
            Action = action,
            EntityName = entityName,
            EntityId =entityId
        };

        await _unitOfWork.AuditLogs.AddAsync(logDetails);
        await _unitOfWork.CompleteAsync();
    }
}
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;
using MyThings.Infrastructure.Repositories;

namespace MyThings.Infrastructure;

public class JobService : IJobService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public JobService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResponse<JobDto>> CreateJob(JobDto job)
    {
        var jobCreation = new Job
        {
            Title = job.Title,
            CanManageAccounts = job.CanManageAccounts,
            CanManageLogistics = job.CanManageLogistics,
            CanManageProducts = job.CanManageProducts
        };

        await _unitOfWork.Jobs.AddAsync(jobCreation);

        await _unitOfWork.CompleteAsync();


        var jobResponse = new JobDto
        {
            Id = jobCreation.Id,
            Title = jobCreation.Title,
            CanManageAccounts = jobCreation.CanManageAccounts,
            CanManageLogistics = jobCreation.CanManageLogistics,
            CanManageProducts = jobCreation.CanManageProducts
        };
        return ServiceResponse<JobDto>.Ok(jobResponse);
    }
}
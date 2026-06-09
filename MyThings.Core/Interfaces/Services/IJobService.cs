using MyThings.Core.DTOs;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces;

public interface IJobService
{
    Task<ServiceResponse<JobDto>> CreateJob(JobDto job); 
}
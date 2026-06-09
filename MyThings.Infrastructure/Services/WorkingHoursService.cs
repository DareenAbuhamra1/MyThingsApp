using System.Runtime.InteropServices;
using Mythings.Core.Interaces.Repositories;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;

namespace MyThings.Infrastructure.Services;

public class WorkingHoursService : IWorkingHoursService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPartnerRepository _partnerRepo;
    private readonly IWorkingHoursRepository _workingHoursRepo;
    public WorkingHoursService(IUnitOfWork unitOfWork, IPartnerRepository partnerRepo, IWorkingHoursRepository workingHoursRepo)
    {
        _unitOfWork = unitOfWork;
        _partnerRepo = partnerRepo;
        _workingHoursRepo = workingHoursRepo;
    }

    public async Task<ServiceResponse<bool>> AddWorkingHoursAsync(WorkingHoursDto dto)
    {
        var jordanZone = TimeZoneInfo.FindSystemTimeZoneById(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Jordan Standard Time" : "Asia/Amman");

        var PartnerWorkingHours = dto.WorkingHours.Select(whDto =>
        {
            DateTime today = DateTime.Today;

            DateTime beginLocal = new DateTime(today.Year, today.Month, today.Day,
                                        whDto.ShiftBegin.Hour, whDto.ShiftBegin.Minute, 0,
                                        DateTimeKind.Unspecified);

            DateTime endLocal = new DateTime(today.Year, today.Month, today.Day,
                                        whDto.ShiftEnd.Hour, whDto.ShiftEnd.Minute, 0,
                                        DateTimeKind.Unspecified);

            DateTime beginUtc = TimeZoneInfo.ConvertTimeToUtc(beginLocal, jordanZone);
            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, jordanZone);

            return new WorkingHour
            {
                PartnerId = dto.PartnerId,
                Day = whDto.Day,
                ShiftBegin = TimeOnly.FromDateTime(beginUtc),
                ShiftEnd = TimeOnly.FromDateTime(endUtc),
            };

        }).ToList();

        var Success = await _workingHoursRepo.AddWorkingHoursAsync(dto.PartnerId, PartnerWorkingHours);
        if (!Success) return ServiceResponse<bool>.Failure("Partner is not found", 404);
        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<WorkingHoursDto>> GetPartnerWorkingHoursAsync(int PartnerId)
    {
        var Partner = await _partnerRepo.GetWorkingHoursAsync(PartnerId);
        if (Partner == null) return ServiceResponse<WorkingHoursDto>.Failure("Partner is not found", 404);

        if (Partner.WorkingHours == null || Partner.WorkingHours.Count == 0)
            return ServiceResponse<WorkingHoursDto>.Ok(new WorkingHoursDto());

        var jordanZone = TimeZoneInfo.FindSystemTimeZoneById(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Jordan Standard Time" : "Asia/Amman");

        var responseDto = new WorkingHoursDto
        {
            PartnerId = Partner.Id,
            WorkingHours = Partner.WorkingHours
            .Select(wh =>
            {
                DateTime today = DateTime.Today;

                DateTime utcBegin = new DateTime(today.Year, today.Month, today.Day,
                                                wh.ShiftBegin.Hour, wh.ShiftBegin.Minute, 0,
                                                DateTimeKind.Utc);

                DateTime utcEnd = new DateTime(today.Year, today.Month, today.Day,
                                            wh.ShiftEnd.Hour, wh.ShiftEnd.Minute, 0,
                                            DateTimeKind.Utc);

    
                DateTime localBegin = TimeZoneInfo.ConvertTimeFromUtc(utcBegin, jordanZone);
                DateTime localEnd = TimeZoneInfo.ConvertTimeFromUtc(utcEnd, jordanZone);

                return new WorkingHourDto
                {
                    Day = wh.Day,
                    ShiftBegin = TimeOnly.FromDateTime(localBegin),
                    ShiftEnd = TimeOnly.FromDateTime(localEnd)
                };
            })
            .OrderBy(wh => wh.Day)
            .ToList()
            };

        return ServiceResponse<WorkingHoursDto>.Ok(responseDto);
    }

    public async Task<ServiceResponse<bool>> ToggleManualCloseAsync(int PartnerId, bool IsManuallyClosed)
    {
        var Partner = await _unitOfWork.Partners.GetByIdAsync(PartnerId);

        if (Partner == null) return ServiceResponse<bool>.Failure("The partner is not found", 404);
        if (!Partner.IsActive) return ServiceResponse<bool>.Failure("The partner account is not active", 400);

        Partner.IsManuallyClosed = IsManuallyClosed;
        Partner.IsAvailable = !IsManuallyClosed;

        Partner.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Partners.Update(Partner);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }
}
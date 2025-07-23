using StudyTracker.Models.DTOs;

namespace StudyTracker.Services;

public interface IReportService
{
    Task<WeeklyReportDto> GetWeeklyReportAsync(DateTime weekStartDate);
    Task<MonthlyReportDto> GetMonthlyReportAsync(int year, int month);
    Task<IEnumerable<ActivitySummaryDto>> GetActivitySummaryByTypeAsync(DateTime startDate, DateTime endDate);
}
using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Models.DTOs;

namespace StudyTracker.Services;

public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityService _activityService;
      
        public ReportService(ApplicationDbContext context, IActivityService activityService)
        {
            _context = context;
            _activityService = activityService;
        }
      
        public async Task<WeeklyReportDto> GetWeeklyReportAsync(DateTime weekStartDate)
        {
            var weekEndDate = weekStartDate.AddDays(6);
            
            var weeklyPlan = await _context.WeeklyPlans
                .FirstOrDefaultAsync(wp => wp.WeekStartDate.Date == weekStartDate.Date);
            
            var activities = await _activityService.GetActivitiesByDateRangeAsync(weekStartDate, weekEndDate);
            var activitiesList = activities.ToList();
            
            var totalPlannedMinutes = weeklyPlan?.PlannedHoursListening + weeklyPlan?.PlannedHoursSpeaking + 
                                    weeklyPlan?.PlannedHoursVocabulary + weeklyPlan?.PlannedHoursImmersion ?? 0;
            var totalCompletedMinutes = activitiesList.Sum(a => a.Duration);
            
            var activitiesByType = activitiesList
                .GroupBy(a => a.Type)
                .Select(g => new ActivitySummaryDto
                {
                    Type = g.Key,
                    CompletedMinutes = g.Sum(a => a.Duration),
                    PlannedMinutes = GetPlannedMinutesByType(weeklyPlan, g.Key),
                    CompletionPercentage = GetPlannedMinutesByType(weeklyPlan, g.Key) > 0 
                        ? (double)g.Sum(a => a.Duration) / GetPlannedMinutesByType(weeklyPlan, g.Key) * 100 
                        : 0
                })
                .ToList();
          
            return new WeeklyReportDto
            {
                WeekStartDate = weekStartDate,
                WeekEndDate = weekEndDate,
                TotalPlannedMinutes = totalPlannedMinutes,
                TotalCompletedMinutes = totalCompletedMinutes,
                CompletionPercentage = totalPlannedMinutes > 0 ? (double)totalCompletedMinutes / totalPlannedMinutes * 100 : 0,
                ActivitiesByType = activitiesByType,
                Activities = activitiesList
            };
        }
      
        public async Task<MonthlyReportDto> GetMonthlyReportAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
          
            var activities = await _activityService.GetActivitiesByDateRangeAsync(startDate, endDate);
            var activitiesList = activities.ToList();
          
            var totalMinutes = activitiesList.Sum(a => a.Duration);
          
            var activitiesByType = activitiesList
                .GroupBy(a => a.Type)
                .Select(g => new ActivitySummaryDto
                {
                    Type = g.Key,
                    CompletedMinutes = g.Sum(a => a.Duration),
                    PlannedMinutes = 0, 
                    CompletionPercentage = 0
                })
                .ToList();
            
            var weeklyReports = new List<WeeklyReportDto>();
            var currentDate = startDate;
          
            while (currentDate <= endDate)
            {
                var weekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                if (weekStart < startDate) weekStart = startDate;
              
                var weeklyReport = await GetWeeklyReportAsync(weekStart);
                weeklyReports.Add(weeklyReport);
              
                currentDate = currentDate.AddDays(7);
            }
          
            return new MonthlyReportDto
            {
                Month = month,
                Year = year,
                TotalMinutes = totalMinutes,
                ActivitiesByType = activitiesByType,
                WeeklyReports = weeklyReports
            };
        }
      
        public async Task<IEnumerable<ActivitySummaryDto>> GetActivitySummaryByTypeAsync(DateTime startDate, DateTime endDate)
        {
            var activities = await _activityService.GetActivitiesByDateRangeAsync(startDate, endDate);
          
            return activities
                .GroupBy(a => a.Type)
                .Select(g => new ActivitySummaryDto
                {
                    Type = g.Key,
                    CompletedMinutes = g.Sum(a => a.Duration),
                    PlannedMinutes = 0,
                    CompletionPercentage = 0
                });
        }
      
        private static int GetPlannedMinutesByType(WeeklyPlan? weeklyPlan, string type)
        {
            if (weeklyPlan == null) return 0;
          
            return type switch
            {
                "Listening" => weeklyPlan.PlannedHoursListening,
                "Speaking" => weeklyPlan.PlannedHoursSpeaking,
                "Vocabulary" => weeklyPlan.PlannedHoursVocabulary,
                "Immersion" => weeklyPlan.PlannedHoursImmersion,
                _ => 0
            };
        }
    }
namespace StudyTracker.Models.DTOs;

public class WeeklyReportDto
{
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public int TotalPlannedMinutes { get; set; }
    public int TotalCompletedMinutes { get; set; }
    public double CompletionPercentage { get; set; }
    public List<ActivitySummaryDto> ActivitiesByType { get; set; } = new();
    public List<ActivityDto> Activities { get; set; } = new();
}
  
public class MonthlyReportDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int TotalMinutes { get; set; }
    public List<ActivitySummaryDto> ActivitiesByType { get; set; } = new();
    public List<WeeklyReportDto> WeeklyReports { get; set; } = new();
}
  
public class ActivitySummaryDto
{
    public string Type { get; set; } = string.Empty;
    public int PlannedMinutes { get; set; }
    public int CompletedMinutes { get; set; }
    public double CompletionPercentage { get; set; }
}
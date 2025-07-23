using StudyTracker.Models.DTOs;

namespace StudyTracker.Services;

public interface IActivityService
{
    Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync();
    Task<IEnumerable<ActivityDto>> GetActivitiesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ActivityDto?> GetActivityByIdAsync(int id);
    Task<ActivityDto> CreateActivityAsync(CreateActivityDto createActivityDto);
    Task<ActivityDto?> UpdateActivityAsync(int id, CreateActivityDto updateActivityDto);
    Task<bool> DeleteActivityAsync(int id);
    Task<IEnumerable<ActivityDto>> GetActivitiesByTypeAsync(string type);
}
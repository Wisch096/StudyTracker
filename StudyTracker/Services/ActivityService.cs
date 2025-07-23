using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Models.DTOs;

namespace StudyTracker.Services;

public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;
      
        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }
      
        public async Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync()
        {
            var activities = await _context.Activities
                .OrderByDescending(a => a.Date)
                .ToListAsync();
              
            return activities.Select(MapToDto);
        }
      
        public async Task<IEnumerable<ActivityDto>> GetActivitiesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var activities = await _context.Activities
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
              
            return activities.Select(MapToDto);
        }
      
        public async Task<ActivityDto?> GetActivityByIdAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            return activity != null ? MapToDto(activity) : null;
        }
      
        public async Task<ActivityDto> CreateActivityAsync(CreateActivityDto createActivityDto)
        {
            var activity = new Activity
            {
                Type = createActivityDto.Type,
                Duration = createActivityDto.Duration,
                Description = createActivityDto.Description,
                Date = createActivityDto.Date
            };
          
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
          
            return MapToDto(activity);
        }
      
        public async Task<ActivityDto?> UpdateActivityAsync(int id, CreateActivityDto updateActivityDto)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return null;
          
            activity.Type = updateActivityDto.Type;
            activity.Duration = updateActivityDto.Duration;
            activity.Description = updateActivityDto.Description;
            activity.Date = updateActivityDto.Date;
          
            await _context.SaveChangesAsync();
          
            return MapToDto(activity);
        }
      
        public async Task<bool> DeleteActivityAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;
          
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
          
            return true;
        }
      
        public async Task<IEnumerable<ActivityDto>> GetActivitiesByTypeAsync(string type)
        {
            var activities = await _context.Activities
                .Where(a => a.Type == type)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
              
            return activities.Select(MapToDto);
        }
      
        private static ActivityDto MapToDto(Activity activity)
        {
            return new ActivityDto
            {
                Id = activity.Id,
                Type = activity.Type,
                Duration = activity.Duration,
                Description = activity.Description,
                Date = activity.Date
            };
        }
    }
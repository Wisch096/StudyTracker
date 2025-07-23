using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models;

public class WeeklyPlan
{
    public int Id { get; set; }
      
    [Required]
    public DateTime WeekStartDate { get; set; }
      
    [Range(0, 10080)] 
    public int PlannedHoursListening { get; set; }
      
    [Range(0, 10080)]
    public int PlannedHoursSpeaking { get; set; }
      
    [Range(0, 10080)]
    public int PlannedHoursVocabulary { get; set; }
      
    [Range(0, 10080)]
    public int PlannedHoursImmersion { get; set; }
      
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
using System.ComponentModel.DataAnnotations;
using StudyTracker.Models.Enum;

namespace StudyTracker.Models;

public class Activity
{
    public int Id { get; set; }
      
    [Required]
    [StringLength(50)]
    public ActivityType Type { get; set; }
      
    [Required]
    [Range(1, 1440)] 
    public int Duration { get; set; }
      
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
      
    [Required]
    public DateTime Date { get; set; }
      
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
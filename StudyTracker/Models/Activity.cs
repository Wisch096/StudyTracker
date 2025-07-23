using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models;

public class Activity
{
    public int Id { get; set; }
      
    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty; // Listening, Speaking, Vocabulary, Immersion
      
    [Required]
    [Range(1, 1440)] // Max 24 hours in minutes
    public int Duration { get; set; }
      
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
      
    [Required]
    public DateTime Date { get; set; }
      
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
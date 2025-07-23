using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models.DTOs;

public class WeeklyPlanDto
{
    public int Id { get; set; }
    public DateTime WeekStartDate { get; set; }
    public int PlannedHoursListening { get; set; }
    public int PlannedHoursSpeaking { get; set; }
    public int PlannedHoursVocabulary { get; set; }
    public int PlannedHoursImmersion { get; set; }
}
  
public class CreateWeeklyPlanDto
{
    [Required(ErrorMessage = "Data de início da semana é obrigatória")]
    public DateTime WeekStartDate { get; set; }
      
    [Range(0, 10080, ErrorMessage = "Horas planejadas devem ser entre 0 e 10080 minutos")]
    public int PlannedHoursListening { get; set; }
      
    [Range(0, 10080, ErrorMessage = "Horas planejadas devem ser entre 0 e 10080 minutos")]
    public int PlannedHoursSpeaking { get; set; }
      
    [Range(0, 10080, ErrorMessage = "Horas planejadas devem ser entre 0 e 10080 minutos")]
    public int PlannedHoursVocabulary { get; set; }
      
    [Range(0, 10080, ErrorMessage = "Horas planejadas devem ser entre 0 e 10080 minutos")]
    public int PlannedHoursImmersion { get; set; }
}
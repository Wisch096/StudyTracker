using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models.DTOs;

public class ActivityDto
{
    public int Id { get; set; }
      
    [Required(ErrorMessage = "Tipo de atividade é obrigatório")]
    public string Type { get; set; } = string.Empty;
      
    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 1440, ErrorMessage = "Duração deve ser entre 1 e 1440 minutos")]
    public int Duration { get; set; }
      
    [StringLength(500, ErrorMessage = "Descrição não pode exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;
      
    [Required(ErrorMessage = "Data é obrigatória")]
    public DateTime Date { get; set; }
}
  
public class CreateActivityDto
{
    [Required(ErrorMessage = "Tipo de atividade é obrigatório")]
    public string Type { get; set; } = string.Empty;
      
    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 1440, ErrorMessage = "Duração deve ser entre 1 e 1440 minutos")]
    public int Duration { get; set; }
      
    [StringLength(500, ErrorMessage = "Descrição não pode exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;
      
    [Required(ErrorMessage = "Data é obrigatória")]
    public DateTime Date { get; set; }
}
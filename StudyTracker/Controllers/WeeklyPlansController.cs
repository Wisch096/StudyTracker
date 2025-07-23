using Microsoft.AspNetCore.Mvc;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Models.DTOs;

namespace StudyTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeeklyPlansController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WeeklyPlansController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeeklyPlanDto>>> GetWeeklyPlans()
    {
        try
        {
            var plans = await _context.WeeklyPlans
                .OrderByDescending(wp => wp.WeekStartDate)
                .ToListAsync();

            var planDtos = plans.Select(MapToDto);

            return Ok(planDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WeeklyPlanDto>> GetWeeklyPlan(int id)
    {
        try
        {
            var plan = await _context.WeeklyPlans.FindAsync(id);

            if (plan == null)
            {
                return NotFound($"Plano semanal com ID {id} não encontrado.");
            }

            return Ok(MapToDto(plan));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpGet("by-date/{weekStartDate}")]
    public async Task<ActionResult<WeeklyPlanDto>> GetWeeklyPlanByDate(DateTime weekStartDate)
    {
        try
        {
            var plan = await _context.WeeklyPlans
                .FirstOrDefaultAsync(wp => wp.WeekStartDate.Date == weekStartDate.Date);

            if (plan == null)
            {
                return NotFound($"Plano semanal para a data {weekStartDate:yyyy-MM-dd} não encontrado.");
            }

            return Ok(MapToDto(plan));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<WeeklyPlanDto>> CreateWeeklyPlan(CreateWeeklyPlanDto createWeeklyPlanDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if plan already exists for this week
            var existingPlan = await _context.WeeklyPlans
                .FirstOrDefaultAsync(wp => wp.WeekStartDate.Date == createWeeklyPlanDto.WeekStartDate.Date);

            if (existingPlan != null)
            {
                return Conflict("Já existe um plano para esta semana.");
            }

            var plan = new WeeklyPlan
            {
                WeekStartDate = createWeeklyPlanDto.WeekStartDate,
                PlannedHoursListening = createWeeklyPlanDto.PlannedHoursListening,
                PlannedHoursSpeaking = createWeeklyPlanDto.PlannedHoursSpeaking,
                PlannedHoursVocabulary = createWeeklyPlanDto.PlannedHoursVocabulary,
                PlannedHoursImmersion = createWeeklyPlanDto.PlannedHoursImmersion
            };

            _context.WeeklyPlans.Add(plan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeeklyPlan), new { id = plan.Id }, MapToDto(plan));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WeeklyPlanDto>> UpdateWeeklyPlan(int id, CreateWeeklyPlanDto updateWeeklyPlanDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var plan = await _context.WeeklyPlans.FindAsync(id);

            if (plan == null)
            {
                return NotFound($"Plano semanal com ID {id} não encontrado.");
            }

            plan.WeekStartDate = updateWeeklyPlanDto.WeekStartDate;
            plan.PlannedHoursListening = updateWeeklyPlanDto.PlannedHoursListening;
            plan.PlannedHoursSpeaking = updateWeeklyPlanDto.PlannedHoursSpeaking;
            plan.PlannedHoursVocabulary = updateWeeklyPlanDto.PlannedHoursVocabulary;
            plan.PlannedHoursImmersion = updateWeeklyPlanDto.PlannedHoursImmersion;
            plan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(plan));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeeklyPlan(int id)
    {
        try
        {
            var plan = await _context.WeeklyPlans.FindAsync(id);

            if (plan == null)
            {
                return NotFound($"Plano semanal com ID {id} não encontrado.");
            }

            _context.WeeklyPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    private static WeeklyPlanDto MapToDto(WeeklyPlan plan)
    {
        return new WeeklyPlanDto
        {
            Id = plan.Id,
            WeekStartDate = plan.WeekStartDate,
            PlannedHoursListening = plan.PlannedHoursListening,
            PlannedHoursSpeaking = plan.PlannedHoursSpeaking,
            PlannedHoursVocabulary = plan.PlannedHoursVocabulary,
            PlannedHoursImmersion = plan.PlannedHoursImmersion
        };
    }
}
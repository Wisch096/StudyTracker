using Microsoft.AspNetCore.Mvc;
using StudyTracker.Models.DTOs;
using StudyTracker.Models.Enum;
using StudyTracker.Services;

namespace StudyTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] ActivityType? type = null)
    {
        try
        {
            IEnumerable<ActivityDto> activities;

            if (type.HasValue)
            {
                activities = await _activityService.GetActivitiesByTypeAsync(type.Value);
            }
            else if (startDate.HasValue && endDate.HasValue)
            {
                activities = await _activityService.GetActivitiesByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                activities = await _activityService.GetAllActivitiesAsync();
            }

            return Ok(activities);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivity(int id)
    {
        try
        {
            var activity = await _activityService.GetActivityByIdAsync(id);

            if (activity == null)
            {
                return NotFound($"Atividade com ID {id} não encontrada.");
            }

            return Ok(activity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto createActivityDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _activityService.CreateActivityAsync(createActivityDto);

            return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActivityDto>> UpdateActivity(int id, CreateActivityDto updateActivityDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _activityService.UpdateActivityAsync(id, updateActivityDto);

            if (activity == null)
            {
                return NotFound($"Atividade com ID {id} não encontrada.");
            }

            return Ok(activity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        try
        {
            var result = await _activityService.DeleteActivityAsync(id);

            if (!result)
            {
                return NotFound($"Atividade com ID {id} não encontrada.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using StudyTracker.Models.DTOs;
using StudyTracker.Services;

namespace StudyTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("weekly")]
    public async Task<ActionResult<WeeklyReportDto>> GetWeeklyReport([FromQuery] DateTime weekStartDate)
    {
        try
        {
            var report = await _reportService.GetWeeklyReportAsync(weekStartDate);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<MonthlyReportDto>> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
    {
        try
        {
            if (month < 1 || month > 12)
            {
                return BadRequest("Mês deve ser entre 1 e 12.");
            }

            if (year < 2020 || year > DateTime.Now.Year + 1)
            {
                return BadRequest("Ano inválido.");
            }

            var report = await _reportService.GetMonthlyReportAsync(year, month);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<IEnumerable<ActivitySummaryDto>>> GetActivitySummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
            {
                return BadRequest("Data de início deve ser anterior à data de fim.");
            }

            var summary = await _reportService.GetActivitySummaryByTypeAsync(startDate, endDate);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
}
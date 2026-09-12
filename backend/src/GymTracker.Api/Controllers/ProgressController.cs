using GymTracker.Api.Extensions;
using GymTracker.Application.DTOs.Progress;
using GymTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/progress")]
public class ProgressController : ControllerBase
{
    private readonly ProgressService _progressService;

    public ProgressController(ProgressService progressService)
    {
        _progressService = progressService;
    }

    [HttpGet]
    public async Task<ActionResult<List<WeeklySummaryDto>>> GetMonthlyProgress([FromQuery] int year, [FromQuery] int month)
    {
        if(month is < 1 or > 12)
        {
            return BadRequest("Month must be between 1 and 12.");
        }

        var summary = await _progressService.GetMonthlyProgressAsync(User.GetUserId(), year, month);

        return Ok(summary);
    }
}
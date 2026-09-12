using GymTracker.Api.Extensions;
using GymTracker.Application.DTOs.Workouts;
using GymTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/workouts")]
public class WorkoutsController: ControllerBase
{
    private readonly WorkoutService _workoutService;

    public WorkoutsController(WorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkoutDto>>> GetAll()
    {
        var workouts = await _workoutService.GetAllAsync(User.GetUserId());

        return Ok(workouts);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutDto>> Create(WorkoutRequest request)
    {
        var workout = await _workoutService.CreateAsync(User.GetUserId(), request);

        return StatusCode(StatusCodes.Status201Created, workout);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<WorkoutDto>> Update(Guid id, WorkoutRequest request)
    {
        var workout = await _workoutService.UpdateAsync(User.GetUserId(), id, request);

        return Ok(workout);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _workoutService.DeleteAsync(User.GetUserId(), id);

        return NoContent();
    }
}
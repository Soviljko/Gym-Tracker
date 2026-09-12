using GymTracker.Application.Abstractions;
using GymTracker.Application.DTOs.Workouts;
using GymTracker.Application.Exceptions;
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Services;

public class WorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;

    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<List<WorkoutDto>> GetAllAsync(Guid userId)
    {
        var workouts = await _workoutRepository.GetAllByUserAsync(userId);

        return workouts.OrderByDescending(w => w.PerformedAt).Select(MapToDto).ToList();
    }

    public async Task<WorkoutDto> CreateAsync(Guid userId, WorkoutRequest request)
    {
        var workout = new Workout
        {
            UserId = userId,
            Type = request.Type,
            PerformedAt = request.PerformedAt,
            DurationMinutes = request.DurationMinutes,
            Calories = request.Calories,
            Intensity = request.Intensity,
            Fatigue = request.Fatigue,
            Notes = request.Notes
        };

        await _workoutRepository.AddAsync(workout);

        return MapToDto(workout);
    }

    public async Task<WorkoutDto> UpdateAsync(Guid userId, Guid workoutId, WorkoutRequest request)
    {
        var workout = await GetOwnedWorkoutAsync(userId, workoutId);

        workout.Type = request.Type;
        workout.PerformedAt = request.PerformedAt;
        workout.DurationMinutes = request.DurationMinutes;
        workout.Calories = request.Calories;
        workout.Intensity = request.Intensity;
        workout.Fatigue = request.Fatigue;
        workout.Notes = request.Notes;

        await _workoutRepository.UpdateAsync(workout);
        return MapToDto(workout);
    }

    public async Task DeleteAsync(Guid userId, Guid workoutId)
    {
        var workout = await GetOwnedWorkoutAsync(userId, workoutId);

        await _workoutRepository.DeleteAsync(workout);
    }

    private async Task<Workout> GetOwnedWorkoutAsync(Guid userId, Guid workoutId)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId);

        if (workout is null || workout.UserId != userId)
        {
            throw new WorkoutNotFoundException();
        }

        return workout;
    }

    private static WorkoutDto MapToDto(Workout workout) => new()
    {
         Id = workout.Id,
        Type = workout.Type,
        PerformedAt = workout.PerformedAt,
        DurationMinutes = workout.DurationMinutes,
        Calories = workout.Calories,
        Intensity = workout.Intensity,
        Fatigue = workout.Fatigue,
        Notes = workout.Notes
    };
    
}
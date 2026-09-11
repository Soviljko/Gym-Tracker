using GymTracker.Application.Abstractions;
using GymTracker.Application.DTOs.Progress;

namespace GymTracker.Application.Services;

public class ProgressService
{
    private readonly IWorkoutRepository _workoutRepository;

    public ProgressService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<List<WeeklySummaryDto>> GetMonthlyProgressAsync(Guid userId, int year, int month)
    {
        var workouts = await _workoutRepository.GetByUserAndMonthAsync(userId, year, month);
        return WeeklySummaryCalculator.Calculate(workouts, year, month);
    }
}

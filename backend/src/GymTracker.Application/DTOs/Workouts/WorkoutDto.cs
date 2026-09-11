using GymTracker.Domain.Enums;

namespace GymTracker.Application.DTOs.Workouts;

public class WorkoutDto
{
    public Guid Id { get; set; }

    public ExerciseType Type { get; set; }

    public DateTime PerformedAt { get; set; }

    public int DurationMinutes { get; set; }

    public int Calories { get; set; }

    public int Intensity { get; set; }

    public int Fatigue { get; set; }

    public string? Notes { get; set; }
}
using GymTracker.Domain.Enums;

namespace GymTracker.Domain.Entities;

public class Workout
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public ExerciseType Type { get; set; }

    public DateTime PerformedAt { get; set; }

    public int DurationMinutes { get; set; }

    public int Calories { get; set; }

    public int Intensity { get; set; }

    public int Fatigue { get; set; }

    public string? Notes { get; set; }
}
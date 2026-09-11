using System.ComponentModel.DataAnnotations;
using GymTracker.Domain.Enums;

namespace GymTracker.Application.DTOs.Workouts;

public class WorkoutRequest
{
    [Required]
    public ExerciseType Type { get; set; }

    [Required]
    public DateTime PerformedAt { get; set; }

    [Range(1, 600)]
    public int DurationMinutes { get; set; }

    [Range(0, 5000)]
    public int Calories { get; set; }

    [Range(1, 10)]
    public int Intensity { get; set; }

    [Range(1, 10)]
    public int Fatigue { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
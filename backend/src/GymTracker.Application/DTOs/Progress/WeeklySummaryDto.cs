namespace GymTracker.Application.DTOs.Progress;

public class WeeklySummaryDto
{
    public DateOnly WeekStart { get; set; }

    public DateOnly WeekEnd { get; set; }

    public int WorkoutCount { get; set; }

    public int TotalDurationMinutes { get; set; }

    public double AverageIntensity { get; set; }

    public double AverageFatigue { get; set; }
}
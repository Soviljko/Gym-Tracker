using GymTracker.Application.DTOs.Progress;
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Services;

public static class WeeklySummaryCalculator
{
    public static List<WeeklySummaryDto> Calculate(IEnumerable<Workout> workouts, int year, int month)
    {
        var monthWorkouts = workouts
            .Where(w => w.PerformedAt.Year == year && w.PerformedAt.Month == month)
            .ToList();

        var firstDay = new DateOnly(year, month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        var summaries = new List<WeeklySummaryDto>();
        var weekStart = StartOfIsoWeek(firstDay);

        while (weekStart <= lastDay)
        {
            var weekEnd = weekStart.AddDays(6);

            var weekWorkouts = monthWorkouts
                .Where(w => DateOnly.FromDateTime(w.PerformedAt) >= weekStart
                         && DateOnly.FromDateTime(w.PerformedAt) <= weekEnd)
                .ToList();

            summaries.Add(new WeeklySummaryDto
            {
                WeekStart = weekStart,
                WeekEnd = weekEnd,
                WorkoutCount = weekWorkouts.Count,
                TotalDurationMinutes = weekWorkouts.Sum(w => w.DurationMinutes),
                AverageIntensity = weekWorkouts.Count > 0 ? weekWorkouts.Average(w => w.Intensity) : 0,
                AverageFatigue = weekWorkouts.Count > 0 ? weekWorkouts.Average(w => w.Fatigue) : 0
            });

            weekStart = weekStart.AddDays(7);
        }

        return summaries;
    }

    private static DateOnly StartOfIsoWeek(DateOnly date)
    {
        int diff = (7 + (int)date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
        return date.AddDays(-diff);
    }
}

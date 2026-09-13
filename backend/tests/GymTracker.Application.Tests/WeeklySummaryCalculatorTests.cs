using GymTracker.Application.Services;
using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;

namespace GymTracker.Application.Tests;

public class WeeklySummaryCalculatorTests
{
    [Fact]
    public void Calculate_EmptyMonth_ReturnsAllWeeksWithZeroCounts()
    {
        var result = WeeklySummaryCalculator.Calculate(new List<Workout>(), 2026, 9);

        Assert.All(result, week =>
        {
            Assert.Equal(0, week.WorkoutCount);
            Assert.Equal(0, week.TotalDurationMinutes);
            Assert.Equal(0, week.AverageIntensity);
            Assert.Equal(0, week.AverageFatigue);
        });
    }

    [Fact]
    public void Calculate_SingleWorkout_LandsInCorrectWeek()
    {
        var workouts = new List<Workout>
        {
            CreateWorkout(new DateTime(2026, 9, 10), durationMinutes: 45, intensity: 7, fatigue: 6)
        };

        var result = WeeklySummaryCalculator.Calculate(workouts, 2026, 9);
        var week = result.Single(w => w.WeekStart == new DateOnly(2026, 9, 7));

        Assert.Equal(1, week.WorkoutCount);
        Assert.Equal(45, week.TotalDurationMinutes);
        Assert.Equal(7, week.AverageIntensity);
        Assert.Equal(6, week.AverageFatigue);
    }

    [Fact]
    public void Calculate_MultipleWorkoutsSameWeek_AggregatesCorrectly()
    {
        var workouts = new List<Workout>
        {
            CreateWorkout(new DateTime(2026, 9, 8), durationMinutes: 30, intensity: 4, fatigue: 4),
            CreateWorkout(new DateTime(2026, 9, 9), durationMinutes: 20, intensity: 8, fatigue: 6)
        };

        var result = WeeklySummaryCalculator.Calculate(workouts, 2026, 9);
        var week = result.Single(w => w.WeekStart == new DateOnly(2026, 9, 7));

        Assert.Equal(2, week.WorkoutCount);
        Assert.Equal(50, week.TotalDurationMinutes);
        Assert.Equal(6, week.AverageIntensity);
        Assert.Equal(5, week.AverageFatigue);
    }

    [Fact]
    public void Calculate_WeekSpanningTwoMonths_ExcludesAdjacentMonthWorkout()
    {
        // Poslednja nedelja septembra 2026 (28.09-04.10) preseca u oktobar.
        var workouts = new List<Workout>
        {
            CreateWorkout(new DateTime(2026, 10, 2), durationMinutes: 60, intensity: 9, fatigue: 9)
        };

        var result = WeeklySummaryCalculator.Calculate(workouts, 2026, 9);
        var lastWeek = result.Single(w => w.WeekStart == new DateOnly(2026, 9, 28));

        Assert.Equal(0, lastWeek.WorkoutCount);
        Assert.Equal(0, lastWeek.TotalDurationMinutes);
    }

    [Fact]
    public void Calculate_FirstAndLastDayOfMonth_FallIntoCorrectWeeks()
    {
        var workouts = new List<Workout>
        {
            CreateWorkout(new DateTime(2026, 9, 1)),
            CreateWorkout(new DateTime(2026, 9, 30))
        };

        var result = WeeklySummaryCalculator.Calculate(workouts, 2026, 9);

        Assert.Equal(1, result.Single(w => w.WeekStart == new DateOnly(2026, 8, 31)).WorkoutCount);
        Assert.Equal(1, result.Single(w => w.WeekStart == new DateOnly(2026, 9, 28)).WorkoutCount);
    }

    private static Workout CreateWorkout(
        DateTime performedAt,
        int durationMinutes = 30,
        int calories = 100,
        int intensity = 5,
        int fatigue = 5) => new()
    {
        UserId = Guid.NewGuid(),
        Type = ExerciseType.Cardio,
        PerformedAt = performedAt,
        DurationMinutes = durationMinutes,
        Calories = calories,
        Intensity = intensity,
        Fatigue = fatigue
    };
}

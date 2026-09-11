namespace GymTracker.Application.Exceptions;

public class WorkoutNotFoundException : Exception
{
    public WorkoutNotFoundException() : base("Workout not found.") { }
}
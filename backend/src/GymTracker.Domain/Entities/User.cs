namespace GymTracker.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}
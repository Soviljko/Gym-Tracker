using GymTracker.Domain.Entities;

namespace GymTracker.Application.Abstractions;

public interface IWorkoutRepository
{
    Task<Workout?> GetByIdAsync(Guid id);

    Task<List<Workout>> GetAllByUserAsync(Guid userId);


    Task<List<Workout>> GetByUserAndMonthAsync(Guid userId, int year, int month);

    Task AddAsync(Workout workout);

    Task UpdateAsync(Workout workout);

    Task DeleteAsync(Workout workout);
}
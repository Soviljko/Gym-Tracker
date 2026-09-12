using GymTracker.Application.Abstractions;
using GymTracker.Domain.Entities;
using GymTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Repositories;

public class WorkoutRepository : IWorkoutRepository
{

    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Workout workout)
    {
        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Workout>> GetAllByUserAsync(Guid userId)
    {
        return await _context.Workouts.Where(w => w.UserId == userId).ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await _context.Workouts.FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Workout>> GetByUserAndMonthAsync(Guid userId, int year, int month)
    {
        return await _context.Workouts
            .Where(w => w.UserId == userId && w.PerformedAt.Year == year && w.PerformedAt.Month == month).ToListAsync();
    }

    public async Task UpdateAsync(Workout workout)
    {
        _context.Workouts.Update(workout);
        await _context.SaveChangesAsync();
    }
}
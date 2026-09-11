using GymTracker.Domain.Entities;

namespace GymTracker.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(string email);

    Task AddAsync(User user);
}
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Abstractions;

public interface ITokenService
{
    string GenerateToken(User user);
}
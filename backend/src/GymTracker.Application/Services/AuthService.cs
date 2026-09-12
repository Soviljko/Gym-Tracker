using GymTracker.Application.Abstractions;
using GymTracker.Application.DTOs.Auth;
using GymTracker.Application.Exceptions;
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            throw new EmailAlreadyExistsException();
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        await _userRepository.AddAsync(user);

        return new AuthResponse
        {
            Token = _tokenService.GenerateToken(user),
            Email = user.Email
        };

    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if(user is null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        return new AuthResponse
        {
            Token = _tokenService.GenerateToken(user),
            Email = user.Email
        };
    }
    
}
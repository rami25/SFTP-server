namespace SFTPPortal.Application.UseCases.Auth;

using SFTPPortal.Application.DTOs;
using SFTPPortal.Domain.Interfaces;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public LoginUseCase(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<LoginResponseDto?> ExecuteAsync(LoginRequestDto request)
    {
        // 1. Find user by username
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || !user.IsActive)
            return null;

        // 2. Verify password
        bool isPasswordValid = _authService.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            return null;

        // 3. Generate JWT token
        var token = _authService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            Entity = user.Entity,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };
    }
}
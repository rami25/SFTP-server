namespace SFTPPortal.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SFTPPortal.Application.DTOs;
using SFTPPortal.Application.UseCases.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly ILogger<AuthController> _logger;

    public AuthController(LoginUseCase loginUseCase, ILogger<AuthController> logger)
    {
        _loginUseCase = loginUseCase;
        _logger = logger;
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username and password are required." });
        }

        var result = await _loginUseCase.ExecuteAsync(request);

        if (result == null)
        {
            _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
            return Unauthorized(new { message = "Invalid username or password." });
        }

        _logger.LogInformation("User {Username} logged in successfully.", request.Username);
        return Ok(result);
    }

    // POST api/auth/logout
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // JWT is stateless — logout is handled on the client side
        // by simply discarding the token
        return Ok(new { message = "Logged out successfully." });
    }
}
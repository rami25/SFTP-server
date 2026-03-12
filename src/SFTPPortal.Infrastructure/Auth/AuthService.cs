namespace SFTPPortal.Infrastructure.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SFTPPortal.Domain.Entities;
using SFTPPortal.Domain.Interfaces;
public class AuthService : IAuthService {
    private readonly string _jwtSecret;
    private readonly int _jwtExpiryHours;

    public AuthService(string jwtSecret, int jwtExpiryHours = 8) {
        _jwtSecret = jwtSecret;
        _jwtExpiryHours = jwtExpiryHours;
    }

    public string GenerateToken(User user) {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("entity", user.Entity),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: "SFTPPortal",
            audience: "SFTPPortal",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtExpiryHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token) {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = "SFTPPortal",
                ValidateAudience = true,
                ValidAudience = "SFTPPortal",
                ValidateLifetime = true
            }, out _);

            return true;
        }
        catch {
            return false;
        }
    }

    public string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool VerifyPassword(string password, string hash) {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
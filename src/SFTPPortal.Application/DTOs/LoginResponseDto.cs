namespace SFTPPortal.Application.DTOs;
public class LoginResponseDto {
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
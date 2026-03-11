namespace SFTPPortal.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;   // Firstname.Lastname
    public string PasswordHash { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;     // e.g. ALMENA, Egypt
    public string Role { get; set; } = string.Empty;       // Admin or User
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
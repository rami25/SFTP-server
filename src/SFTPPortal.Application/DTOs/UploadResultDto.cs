namespace SFTPPortal.Application.DTOs;

public class UploadResultDto
{
    public bool Success { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? ErrorReason { get; set; }
}
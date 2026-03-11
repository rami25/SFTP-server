namespace SFTPPortal.Application.DTOs;

public class FileItemDto
{
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; }
    public string SizeFormatted { get; set; } = string.Empty; // e.g. "1.2 MB"
    public DateTime LastModified { get; set; }
    public bool IsDirectory { get; set; }
    public string FullPath { get; set; } = string.Empty;
}
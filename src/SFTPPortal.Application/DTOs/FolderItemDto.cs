namespace SFTPPortal.Application.DTOs;
public class FolderItemDto {
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Demographic / Bank / GL
    public string Entity { get; set; } = string.Empty;
    public string RemotePath { get; set; } = string.Empty;
    public bool CanUpload { get; set; }
    public bool CanDownload { get; set; }
}
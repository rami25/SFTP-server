namespace SFTPPortal.Domain.Entities;
public class FileItem {
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsDirectory { get; set; }
    public string FullPath { get; set; } = string.Empty;
}
namespace SFTPPortal.Domain.Entities;
using SFTPPortal.Domain.Enums;

public class FolderItem {
    public string Name { get; set; } = string.Empty;
    public FolderType Type { get; set; }
    public string Entity { get; set; } = string.Empty;  
    public string RemotePath { get; set; } = string.Empty; // path on SFTP server
    public bool CanUpload => Type == FolderType.Demographic;
    public bool CanDownload => Type == FolderType.Bank || Type == FolderType.GL;
}
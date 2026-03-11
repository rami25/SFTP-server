namespace SFTPPortal.Domain.Interfaces;

using SFTPPortal.Domain.Entities;

public interface ISftpService
{
    Task<List<FileItem>> ListFilesAsync(string remotePath);
    Task UploadFileAsync(string remotePath, Stream fileStream, string fileName);
    Task<Stream> DownloadFileAsync(string remotePath, string fileName);
    Task<bool> TestConnectionAsync();
}
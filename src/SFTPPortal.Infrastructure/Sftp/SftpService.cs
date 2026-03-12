namespace SFTPPortal.Infrastructure.Sftp;
using Renci.SshNet;
using SFTPPortal.Domain.Entities;
using SFTPPortal.Domain.Interfaces;
public class SftpService : ISftpService, IDisposable {
    private readonly SftpClient _client;
    private readonly string _rootPath = "/SFTPRoot"; // najem nbadel ba3ed

    public SftpService(string host, int port, string username, string password) {
        _client = new SftpClient(host, port, username, password);
    }

    private void EnsureConnected() { // connection helpers
        if (!_client.IsConnected)
            _client.Connect();
    }

    public async Task<bool> TestConnectionAsync() {
        return await Task.Run(() =>
        {
            try
            {
                EnsureConnected();
                return _client.IsConnected;
            }
            catch
            {
                return false;
            }
        });
    }

    public async Task<List<FileItem>> ListFilesAsync(string remotePath) { // list files
        return await Task.Run(() =>
        {
            EnsureConnected();
            var fullPath = $"{_rootPath}{remotePath}";

            return _client.ListDirectory(fullPath)
                .Where(f => !f.Name.StartsWith("."))
                .Select(f => new FileItem
                {
                    Name = f.Name,
                    Size = f.Length,
                    LastModified = f.LastWriteTime,
                    IsDirectory = f.IsDirectory,
                    FullPath = f.FullName
                })
                .ToList();
        });
    }

    public async Task UploadFileAsync(string remotePath, Stream fileStream, string fileName) { // upload file
        await Task.Run(() =>
        {
            EnsureConnected();
            var fullRemotePath = $"{_rootPath}{remotePath}/{fileName}";
            _client.UploadFile(fileStream, fullRemotePath);
        });
    }

    public async Task<Stream> DownloadFileAsync(string remotePath, string fileName) { // download file
        return await Task.Run(() =>
        {
            EnsureConnected();
            var fullRemotePath = $"{_rootPath}{remotePath}/{fileName}";
            var memoryStream = new MemoryStream();
            _client.DownloadFile(fullRemotePath, memoryStream);
            memoryStream.Position = 0;
            return (Stream)memoryStream;
        });
    }
    public void Dispose() { // cleanup 
        if (_client.IsConnected)
            _client.Disconnect();
        _client.Dispose();
    }
}
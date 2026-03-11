namespace SFTPPortal.Application.UseCases.Files;

using SFTPPortal.Application.DTOs;
using SFTPPortal.Domain.Interfaces;

public class ListFilesUseCase
{
    private readonly ISftpService _sftpService;

    public ListFilesUseCase(ISftpService sftpService)
    {
        _sftpService = sftpService;
    }

    public async Task<List<FileItemDto>> ExecuteAsync(string remotePath)
    {
        var files = await _sftpService.ListFilesAsync(remotePath);

        return files.Select(f => new FileItemDto
        {
            Name = f.Name,
            Size = f.Size,
            SizeFormatted = FormatSize(f.Size),
            LastModified = f.LastModified,
            IsDirectory = f.IsDirectory,
            FullPath = f.FullPath
        }).ToList();
    }

    private string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024):F1} MB";
    }
}
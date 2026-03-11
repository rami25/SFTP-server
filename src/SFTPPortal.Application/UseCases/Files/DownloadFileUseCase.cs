namespace SFTPPortal.Application.UseCases.Files;

using SFTPPortal.Domain.Interfaces;

public class DownloadFileUseCase
{
    private readonly ISftpService _sftpService;

    public DownloadFileUseCase(ISftpService sftpService)
    {
        _sftpService = sftpService;
    }

    public async Task<Stream> ExecuteAsync(string remotePath, string fileName)
    {
        return await _sftpService.DownloadFileAsync(remotePath, fileName);
    }
}
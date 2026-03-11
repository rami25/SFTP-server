namespace SFTPPortal.Application.UseCases.Files;

using SFTPPortal.Application.DTOs;
using SFTPPortal.Domain.Interfaces;

public class UploadFileUseCase
{
    private readonly ISftpService _sftpService;
    private readonly IFileNamingService _namingService;

    public UploadFileUseCase(ISftpService sftpService, IFileNamingService namingService)
    {
        _sftpService = sftpService;
        _namingService = namingService;
    }

    public async Task<UploadResultDto> ExecuteAsync(string remotePath, Stream fileStream, string fileName)
    {
        // 1. Validate naming convention first
        if (!_namingService.IsValidFileName(fileName))
        {
            return new UploadResultDto
            {
                Success = false,
                FileName = fileName,
                ErrorReason = _namingService.GetRejectionReason(fileName)
            };
        }

        // 2. Upload to SFTP
        await _sftpService.UploadFileAsync(remotePath, fileStream, fileName);

        return new UploadResultDto
        {
            Success = true,
            FileName = fileName
        };
    }
}
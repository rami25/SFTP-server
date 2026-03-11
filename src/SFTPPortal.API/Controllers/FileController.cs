namespace SFTPPortal.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SFTPPortal.Application.UseCases.Files;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly ListFilesUseCase _listFilesUseCase;
    private readonly UploadFileUseCase _uploadFileUseCase;
    private readonly DownloadFileUseCase _downloadFileUseCase;
    private readonly ILogger<FileController> _logger;

    public FileController(
        ListFilesUseCase listFilesUseCase,
        UploadFileUseCase uploadFileUseCase,
        DownloadFileUseCase downloadFileUseCase,
        ILogger<FileController> logger)
    {
        _listFilesUseCase = listFilesUseCase;
        _uploadFileUseCase = uploadFileUseCase;
        _downloadFileUseCase = downloadFileUseCase;
        _logger = logger;
    }

    // GET api/files?remotePath=/Demographic ALMENA
    [HttpGet]
    public async Task<IActionResult> ListFiles([FromQuery] string remotePath)
    {
        if (string.IsNullOrWhiteSpace(remotePath))
            return BadRequest(new { message = "Remote path is required." });

        var files = await _listFilesUseCase.ExecuteAsync(remotePath);
        return Ok(files);
    }

    // POST api/files/upload?remotePath=/Demographic ALMENA
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
        [FromQuery] string remotePath,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file was provided." });

        if (string.IsNullOrWhiteSpace(remotePath))
            return BadRequest(new { message = "Remote path is required." });

        using var stream = file.OpenReadStream();
        var result = await _uploadFileUseCase.ExecuteAsync(remotePath, stream, file.FileName);

        if (!result.Success)
        {
            _logger.LogWarning("File upload rejected: {Reason}", result.ErrorReason);
            return BadRequest(new { message = result.ErrorReason });
        }

        _logger.LogInformation("File uploaded: {FileName} to {Path}", file.FileName, remotePath);
        return Ok(result);
    }

    // GET api/files/download?remotePath=/Bank ALMENA&fileName=Sample.pgp
    [HttpGet("download")]
    public async Task<IActionResult> Download(
        [FromQuery] string remotePath,
        [FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(remotePath) || string.IsNullOrWhiteSpace(fileName))
            return BadRequest(new { message = "Remote path and file name are required." });

        var stream = await _downloadFileUseCase.ExecuteAsync(remotePath, fileName);

        _logger.LogInformation("File downloaded: {FileName} from {Path}", fileName, remotePath);

        // Return file as download
        return File(stream, "application/octet-stream", fileName);
    }
}
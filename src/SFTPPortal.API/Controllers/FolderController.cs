namespace SFTPPortal.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SFTPPortal.Application.UseCases.Folders;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase {
    private readonly ListFoldersUseCase _listFoldersUseCase;
    private readonly ILogger<FolderController> _logger;

    public FolderController(ListFoldersUseCase listFoldersUseCase, ILogger<FolderController> logger) {
        _listFoldersUseCase = listFoldersUseCase;
        _logger = logger;
    }

    // GET api/folders/{entity}
    [HttpGet("{entity}")]
    public IActionResult GetFolders(string entity) {
        if (string.IsNullOrWhiteSpace(entity))
            return BadRequest(new { message = "Entity name is required." });

        var folders = _listFoldersUseCase.Execute(entity);

        _logger.LogInformation("Folders listed for entity: {Entity}", entity);
        return Ok(folders);
    }
}
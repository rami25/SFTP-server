namespace SFTPPortal.Application.UseCases.Folders;
using SFTPPortal.Application.DTOs;
using SFTPPortal.Domain.Enums;
public class ListFoldersUseCase {
    public List<FolderItemDto> Execute(string entity) {
        var folders = new List<FolderItemDto> { // najem nbadel ba3ed
            new FolderItemDto {
                Name = $"Demographic {entity}",
                Type = FolderType.Demographic.ToString(),
                Entity = entity,
                RemotePath = $"/Demographic {entity}",
                CanUpload = true,
                CanDownload = false
            },
            new FolderItemDto {
                Name = $"Bank {entity}",
                Type = FolderType.Bank.ToString(),
                Entity = entity,
                RemotePath = $"/Bank {entity}",
                CanUpload = false,
                CanDownload = true
            }
        };

        // valable l marocco as exemple
        if (entity.Equals("Morocco", StringComparison.OrdinalIgnoreCase)) {
            folders.Add(new FolderItemDto {
                Name = $"GL {entity}",
                Type = FolderType.GL.ToString(),
                Entity = entity,
                RemotePath = $"/GL {entity}",
                CanUpload = false,
                CanDownload = true
            });
        }
        return folders;
    }
}
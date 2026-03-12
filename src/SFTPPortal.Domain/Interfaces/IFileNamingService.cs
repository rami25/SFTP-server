namespace SFTPPortal.Domain.Interfaces;
public interface IFileNamingService {
    bool IsValidFileName(string fileName);
    string GetRejectionReason(string fileName);
}
namespace SFTPPortal.Application.Validators;
using SFTPPortal.Domain.Interfaces;
using System.Text.RegularExpressions;
public class FileNameValidator : IFileNamingService {
    // CCCC_EEEEEEE_IN_ISO_SS_FFFFFFFFFFFFFFFFFFFF_DDMMYYYY.csv
    // exp: ALIQ_101k8wp_IN_EGY_01_DemoFile_22062020.csv
    private static readonly Regex _pattern = new Regex(
        @"^[A-Z]{2,4}_[A-Za-z0-9]+_IN_[A-Z]{3}_\d{2}_[A-Za-z0-9]+_\d{8}\.csv$",
        RegexOptions.Compiled
    );

    public bool IsValidFileName(string fileName) {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;
        return _pattern.IsMatch(fileName);
    }

    public string GetRejectionReason(string fileName) {
        if (string.IsNullOrWhiteSpace(fileName))
            return "File name cannot be empty.";

        if (!fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return "File must have a .csv extension.";

        var parts = fileName.Replace(".csv", "").Split('_');

        if (parts.Length < 7)
            return "File name must have all segments: CLIENT_ENV_IN_ISO_SEQ_NAME_DATE";

        if (parts[2] != "IN")
            return "Third segment must be 'IN' (Inbound).";

        if (parts[3].Length != 3)
            return "Fourth segment must be a 3-letter ISO country code (e.g. EGY, MAR).";

        if (!Regex.IsMatch(parts[4], @"^\d{2}$"))
            return "Fifth segment must be a 2-digit sequence number (e.g. 01, 02).";

        if (!Regex.IsMatch(parts[^1], @"^\d{8}$"))
            return "Last segment must be a date in DDMMYYYY format (e.g. 22062020).";

        return "File name does not match the required naming convention.";
    }
}
using System.Text.RegularExpressions;

namespace EAxWiki.Core.Models;

public class EaRepository
{
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public List<EaPackage> RootPackages { get; set; } = new();

    public static string Redact(string connectionString)
    {
        if (!connectionString.Contains('=')) return connectionString;
        return Regex.Replace(
            connectionString,
            @"(?i)(Password|Pwd|User\s*Id|Uid|User\s*Name|Username)\s*=[^;]*",
            m => m.Groups[1].Value + "=***");
    }
}

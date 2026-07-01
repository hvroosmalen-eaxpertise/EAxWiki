using System.Security.Cryptography;

namespace EAxWiki.Export.Helpers;

/// <summary>
/// Generates and persists a per-instance shared secret that gates the wiki write-back API.
/// Stored as a plain file inside the wiki output directory (gitignored) so the exporter (which
/// embeds it into the generated widgets) and the separately-started --api server (which validates
/// it on every request) agree on the same value without any manual configuration.
/// </summary>
public static class ApiTokenStore
{
    private const string FileName = ".eaxwiki-token";

    public static string GetOrCreate(string outputPath)
    {
        var path = Path.Combine(outputPath, FileName);
        if (File.Exists(path))
        {
            var existing = File.ReadAllText(path).Trim();
            if (existing.Length > 0) return existing;
        }

        Directory.CreateDirectory(outputPath);
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(24)).ToLowerInvariant();
        File.WriteAllText(path, token);
        return token;
    }
}

using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

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

    public static string GetOrCreate(string outputPath, ILogger? logger = null)
    {
        var path = Path.Combine(outputPath, FileName);
        var exists = File.Exists(path);
        if (exists)
        {
            var existing = File.ReadAllText(path).Trim();
            if (existing.Length > 0)
            {
                logger?.LogInformation("Token: read from {Path} ({Token})", path, Redact(existing));
                return existing;
            }
        }

        Directory.CreateDirectory(outputPath);
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(24)).ToLowerInvariant();
        File.WriteAllText(path, token);
        logger?.LogInformation("Token: created at {Path} ({Status}) Value={Token}", path, exists ? "overwritten (was empty)" : "new file", Redact(token));
        return token;
    }

    // Log-safe fingerprint — first 4 + last 4 of the hex token. Full token in log or 401 body
    // would defeat the point of the FixedTimeEquals compare on the request path.
    private static string Redact(string token) =>
        token.Length <= 8 ? "…" : $"{token[..4]}…{token[^4..]}";
}

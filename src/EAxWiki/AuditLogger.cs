using System.Text.Json;

namespace EAxWiki;

internal static class AuditLogger
{
    private static readonly string LogDir = Path.Combine(
        Directory.GetCurrentDirectory(), ".eaxwiki-monitor");

    public static async Task LogAsync(
        string outputPath,
        string endpoint,
        int? elementId,
        string? field,
        int statusCode,
        string? message,
        string? tokenPrefix)
    {
        try
        {
            Directory.CreateDirectory(LogDir);
            var entry = new
            {
                timestamp = DateTime.UtcNow.ToString("O"),
                tokenPrefix = (tokenPrefix ?? "").Length > 8 ? tokenPrefix![..8] : (tokenPrefix ?? ""),
                endpoint,
                elementId,
                field,
                statusCode,
                message
            };
            var line = JsonSerializer.Serialize(entry);
            var logPath = Path.Combine(LogDir, "audit.log");
            await File.AppendAllTextAsync(logPath, line + Environment.NewLine);
        }
        catch
        {
            // Best-effort — never let audit logging failure block a write-back
        }
    }
}

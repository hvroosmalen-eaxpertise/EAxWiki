using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

/// <summary>
/// Reads <c>.data/edit-lock.json</c> (relative to the wiki dir's parent): absent → unlocked;
/// expired → stale lock removed and reported unlocked; active → export defers this cycle.
/// </summary>
public static class EditLock
{
    public static bool IsActive(string wikiDir)
    {
        var lockPath = Path.Combine(
            Path.Combine(Path.GetDirectoryName(wikiDir) ?? string.Empty, ".data"), "edit-lock.json");
        if (!File.Exists(lockPath)) return false;

        try
        {
            var doc = JsonDocument.Parse(File.ReadAllText(lockPath));
            var root = doc.RootElement;
            if (!root.TryGetProperty("Active", out var activeEl) || !activeEl.GetBoolean())
                return false;

            if (root.TryGetProperty("ExpiresAt", out var expiresEl) &&
                DateTimeOffset.TryParse(expiresEl.GetString(), out var expires))
            {
                if (DateTimeOffset.UtcNow > expires)
                {
                    File.Delete(lockPath);
                    return false;
                }
            }
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false; // valid JSON with a non-object root (e.g. an array or scalar)
        }
    }
}
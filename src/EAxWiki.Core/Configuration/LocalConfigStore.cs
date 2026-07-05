using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EAxWiki.Core.Configuration;

/// <summary>
/// Encrypts the saved EA repository connection string (which may embed a DB password, e.g. for a
/// SQL Server-backed repo) and optional Slack webhook URL at rest, using Windows DPAPI scoped to
/// the current user account — nothing else on the machine, and no other Windows user, can decrypt it.
/// Transparently reads a pre-existing plaintext ".eaxwiki" file (from before this existed) and
/// re-encrypts it on the next save.
///
/// Format: encrypted JSON with "repoPath" (required) and "webhookUrl" (optional) fields.
/// Legacy format: plaintext connection string (migrated to JSON on next save).
///
/// Lives in EAxWiki.Core (not the EAxWiki console/API project) so it can be shared with other
/// front ends — e.g. EAxWiki.SchedulerUI reads it read-only to display the current repo/port
/// configuration alongside scheduling controls.
/// </summary>
public static class LocalConfigStore
{
    // Defense-in-depth only, not a secret: narrows DPAPI decryption to blobs this app wrote,
    // rather than any DPAPI-protected value for this user account.
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("EAxWiki.LocalConfig.v1");

    public class Config
    {
        public string? RepoPath { get; set; }
        public string? WebhookUrl { get; set; }
        public string? TeamsWebhookUrl { get; set; }
        public int? WikiPort { get; set; }
        public int? ApiPort { get; set; }
    }

    public static Config Load(string path, out bool wasLegacyPlaintext)
    {
        var raw = File.ReadAllText(path).Trim();
        wasLegacyPlaintext = false;

        byte[] encrypted;
        try
        {
            encrypted = Convert.FromBase64String(raw);
        }
        catch (FormatException)
        {
            // Not base64 — try as plaintext JSON first, then fall back to legacy format
            try
            {
                var config = JsonSerializer.Deserialize<Config>(raw);
                if (config != null && !string.IsNullOrEmpty(config.RepoPath))
                    return config;
            }
            catch (JsonException) { }

            // Legacy plaintext format: just the connection string
            wasLegacyPlaintext = true;
            return new Config { RepoPath = raw };
        }

        try
        {
            var decrypted = ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.CurrentUser);
            var json = Encoding.UTF8.GetString(decrypted);
            return JsonSerializer.Deserialize<Config>(json) ?? new Config { RepoPath = json };
        }
        catch (CryptographicException)
        {
            // Valid base64 but not a DPAPI blob this user/app can unprotect — treat as plaintext
            // (e.g. a legacy value that happened to be base64-shaped).
            wasLegacyPlaintext = true;
            return new Config { RepoPath = raw };
        }
        catch (JsonException)
        {
            // Decrypted successfully but isn't valid JSON — must be old encrypted format (just the string).
            // Treat as legacy and return it as the repo path.
            return new Config { RepoPath = raw };
        }
    }

    public static void Save(string path, Config config)
    {
        var json = JsonSerializer.Serialize(config);
        var plainBytes = Encoding.UTF8.GetBytes(json);
        var encrypted = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser);
        File.WriteAllText(path, Convert.ToBase64String(encrypted));
    }
}

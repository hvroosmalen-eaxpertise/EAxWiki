using System.Security.Cryptography;
using System.Text;

namespace EAxWiki;

/// <summary>
/// Encrypts the saved EA repository connection string (which may embed a DB password, e.g. for a
/// SQL Server-backed repo) at rest, using Windows DPAPI scoped to the current user account — nothing
/// else on the machine, and no other Windows user, can decrypt it. Transparently reads a pre-existing
/// plaintext ".eaxwiki" file (from before this existed) and re-encrypts it on the next save.
/// </summary>
public static class LocalConfigStore
{
    // Defense-in-depth only, not a secret: narrows DPAPI decryption to blobs this app wrote,
    // rather than any DPAPI-protected value for this user account.
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("EAxWiki.LocalConfig.v1");

    public static string Load(string path, out bool wasLegacyPlaintext)
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
            // Not base64 — a plaintext file predating encryption.
            wasLegacyPlaintext = true;
            return raw;
        }

        try
        {
            var decrypted = ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }
        catch (CryptographicException)
        {
            // Valid base64 but not a DPAPI blob this user/app can unprotect — treat as plaintext
            // (e.g. a legacy value that happened to be base64-shaped).
            wasLegacyPlaintext = true;
            return raw;
        }
    }

    public static void Save(string path, string value)
    {
        var plainBytes = Encoding.UTF8.GetBytes(value);
        var encrypted = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser);
        File.WriteAllText(path, Convert.ToBase64String(encrypted));
    }
}

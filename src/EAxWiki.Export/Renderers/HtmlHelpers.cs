using System.Security.Cryptography;
using System.Text;

namespace EAxWiki.Export.Renderers;

internal static class HtmlHelpers
{
    internal static string HtmlEscape(string s) =>
        (s ?? string.Empty).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#39;");

    internal static string JsonEscape(string s) =>
        (s ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", " ").Replace("\t", " ");

    internal static string ComputeStatusHash(string status)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(status ?? string.Empty));
        return Convert.ToHexString(bytes)[..8].ToLowerInvariant();
    }

    internal static string ComputeNotesHash(string? notes)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(notes ?? string.Empty));
        return Convert.ToHexString(bytes)[..8].ToLowerInvariant();
    }
}

using System.Reflection;
using System.Text;

namespace EAxWiki.Export.Helpers;

/// <summary>
/// Reads a text embedded resource by trailing-name match. Every string is cached for the process
/// lifetime — resources are baked into the assembly, so re-reading is pure waste. Callers must not
/// mutate the returned string.
/// </summary>
internal static class EmbeddedResource
{
    private static readonly Dictionary<string, string> _cache = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Lock _gate = new();

    internal static string ReadText(string fileName)
    {
        lock (_gate)
        {
            if (_cache.TryGetValue(fileName, out var cached)) return cached;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException(
                    $"Embedded resource '{fileName}' not found. Check EAxWiki.Export.csproj EmbeddedResource entries.");

            using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var text = reader.ReadToEnd();
            _cache[fileName] = text;
            return text;
        }
    }
}

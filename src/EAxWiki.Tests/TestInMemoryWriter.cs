using EAxWiki.Core.Interfaces;

namespace EAxWiki.Tests;

internal sealed class TestInMemoryWriter : IOutputWriter
{
    public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
    public readonly HashSet<string> Directories = new(StringComparer.OrdinalIgnoreCase);

    public Task CreateDirectoryAsync(string path, CancellationToken ct = default) { Directories.Add(path); return Task.CompletedTask; }
    public Task WriteFileAsync(string filePath, string content, CancellationToken ct = default) { Files[Normalize(filePath)] = content; return Task.CompletedTask; }

    private static string Normalize(string path) => path.Replace('\\', '/');
}

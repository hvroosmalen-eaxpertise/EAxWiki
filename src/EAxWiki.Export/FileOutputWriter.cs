using EAxWiki.Core.Interfaces;

namespace EAxWiki.Export;

public class FileOutputWriter : IOutputWriter
{
    public Task CreateDirectoryAsync(string path, CancellationToken ct = default)
    {
        Directory.CreateDirectory(path);
        return Task.CompletedTask;
    }

    public async Task WriteFileAsync(string filePath, string content, CancellationToken ct = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await File.WriteAllTextAsync(filePath, content, ct);
    }
}

namespace EAxWiki.Core.Interfaces;

public interface IOutputWriter
{
    Task CreateDirectoryAsync(string path, CancellationToken ct = default);
    Task WriteFileAsync(string filePath, string content, CancellationToken ct = default);
}

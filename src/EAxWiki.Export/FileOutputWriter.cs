using EAxWiki.Core.Interfaces;

namespace EAxWiki.Export;

public class FileOutputWriter : IOutputWriter
{
    public Task CreateDirectoryAsync(string path)
    {
        Directory.CreateDirectory(path);
        return Task.CompletedTask;
    }

    public async Task WriteFileAsync(string filePath, string content)
    {
        await File.WriteAllTextAsync(filePath, content);
    }
}

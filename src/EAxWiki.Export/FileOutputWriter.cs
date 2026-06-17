using EAxWiki.Core.Interfaces;

namespace EAxWiki.Export;

public class FileOutputWriter : IOutputWriter
{
    public Task CreateDirectoryAsync(string path)
    {
        Directory.CreateDirectory(path);
        return Task.CompletedTask;
    }

    public Task WriteFileAsync(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
        return Task.CompletedTask;
    }
}

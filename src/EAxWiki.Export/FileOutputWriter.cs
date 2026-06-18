using System.Collections.Concurrent;
using EAxWiki.Core.Interfaces;

namespace EAxWiki.Export;

public class FileOutputWriter : IOutputWriter
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new();

    public Task CreateDirectoryAsync(string path)
    {
        Directory.CreateDirectory(path);
        return Task.CompletedTask;
    }

    public async Task WriteFileAsync(string filePath, string content)
    {
        var fileLock = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        await fileLock.WaitAsync();
        try
        {
            await File.WriteAllTextAsync(filePath, content);
        }
        finally
        {
            fileLock.Release();
        }
    }
}

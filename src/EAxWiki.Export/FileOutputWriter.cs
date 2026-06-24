using System.Collections.Concurrent;
using EAxWiki.Core.Interfaces;

namespace EAxWiki.Export;

public class FileOutputWriter : IOutputWriter, IDisposable
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new();
    private bool _disposed;

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
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await File.WriteAllTextAsync(filePath, content);
        }
        finally
        {
            fileLock.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        foreach (var sem in _fileLocks.Values)
            sem.Dispose();
        _fileLocks.Clear();
        GC.SuppressFinalize(this);
    }
}

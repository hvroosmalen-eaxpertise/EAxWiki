using EAxWiki.Export;

namespace EAxWiki.Tests;

public class FileOutputWriterTests : IDisposable
{
    private readonly string _dir;

    public FileOutputWriterTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_writer_" + Guid.NewGuid().ToString("N"));
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public async Task CreateDirectoryAsync_CreatesDirectory()
    {
        var sub = Path.Combine(_dir, "a", "b", "c");
        await new FileOutputWriter().CreateDirectoryAsync(sub);
        Assert.True(Directory.Exists(sub));
    }

    [Fact]
    public async Task CreateDirectoryAsync_ExistingDirectory_DoesNotThrow()
    {
        Directory.CreateDirectory(_dir);
        await new FileOutputWriter().CreateDirectoryAsync(_dir);
    }

    [Fact]
    public async Task WriteFileAsync_CreatesFileWithContent()
    {
        var path = Path.Combine(_dir, "test.md");
        await new FileOutputWriter().WriteFileAsync(path, "hello");
        Assert.True(File.Exists(path));
        Assert.Equal("hello", await File.ReadAllTextAsync(path));
    }

    [Fact]
    public async Task WriteFileAsync_CreatesIntermediateDirectories()
    {
        var path = Path.Combine(_dir, "a", "b", "test.md");
        await new FileOutputWriter().WriteFileAsync(path, "nested");
        Assert.True(Directory.Exists(Path.Combine(_dir, "a", "b")));
        Assert.True(File.Exists(path));
    }

    [Fact]
    public async Task WriteFileAsync_OverwritesExistingFile()
    {
        var path = Path.Combine(_dir, "test.md");
        await new FileOutputWriter().WriteFileAsync(path, "first");
        await new FileOutputWriter().WriteFileAsync(path, "second");
        Assert.Equal("second", await File.ReadAllTextAsync(path));
    }

    [Fact]
    public async Task WriteFileAsync_EmptyContent_WritesEmptyFile()
    {
        var path = Path.Combine(_dir, "empty.md");
        await new FileOutputWriter().WriteFileAsync(path, "");
        Assert.Equal("", await File.ReadAllTextAsync(path));
    }

    [Fact]
    public async Task WriteFileAsync_CancelledToken_Throws()
    {
        var path = Path.Combine(_dir, "cancel.md");
        var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            new FileOutputWriter().WriteFileAsync(path, "test", cts.Token));
    }
}

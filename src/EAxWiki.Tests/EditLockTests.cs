using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class EditLockTests : IDisposable
{
    private readonly string _dir;

    public EditLockTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_lock_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private string WriteLock(bool active, DateTimeOffset expiresAt)
    {
        var lockDir = Path.Combine(_dir, ".data");
        Directory.CreateDirectory(lockDir);
        var lockPath = Path.Combine(lockDir, "edit-lock.json");
        File.WriteAllText(lockPath, System.Text.Json.JsonSerializer.Serialize(new { Active = active, ExpiresAt = expiresAt.ToString("O") }));
        return lockPath;
    }

    [Fact]
    public void IsActive_NoLockFile_ReturnsFalse()
    {
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_InactiveLock_ReturnsFalse()
    {
        WriteLock(false, DateTimeOffset.UtcNow.AddMinutes(5));
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_ActiveUnExpired_ReturnsTrue()
    {
        WriteLock(true, DateTimeOffset.UtcNow.AddMinutes(5));
        Assert.True(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_ExpiredLock_RemovesFileAndReturnsFalse()
    {
        var lockPath = WriteLock(true, DateTimeOffset.UtcNow.AddMinutes(-5));
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
        Assert.False(File.Exists(lockPath), "expired lock file should be removed");
    }

    [Fact]
    public void IsActive_CorruptLock_ReturnsFalse()
    {
        var lockDir = Path.Combine(_dir, ".data");
        Directory.CreateDirectory(lockDir);
        File.WriteAllText(Path.Combine(lockDir, "edit-lock.json"), "{corrupt");
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_NonObjectJson_ReturnsFalse()
    {
        var lockDir = Path.Combine(_dir, ".data");
        Directory.CreateDirectory(lockDir);
        File.WriteAllText(Path.Combine(lockDir, "edit-lock.json"), "[1, 2, 3]");
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }
}
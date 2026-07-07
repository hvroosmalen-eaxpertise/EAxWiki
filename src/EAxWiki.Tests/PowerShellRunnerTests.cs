using EAxWiki.SchedulerUI;

namespace EAxWiki.Tests;

public class PowerShellRunnerTests
{
    [Fact]
    public void GetFullPathFromPathEnv_FindsFileInPath()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "eaxwiki_ps_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(tempDir);
            var markerPath = Path.Combine(tempDir, "pwsh.exe");
            File.WriteAllText(markerPath, "");

            var result = PowerShellRunner.GetFullPathFromPathEnv("pwsh.exe", [tempDir]);
            Assert.NotNull(result);
            Assert.Equal(markerPath, result, StringComparer.OrdinalIgnoreCase);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void GetFullPathFromPathEnv_NotFound_ReturnsNull()
    {
        var result = PowerShellRunner.GetFullPathFromPathEnv("nonexistent.exe", ["C:\\Windows"]);
        Assert.Null(result);
    }

    [Fact]
    public void GetFullPathFromPathEnv_EmptyPaths_ReturnsNull()
    {
        var result = PowerShellRunner.GetFullPathFromPathEnv("pwsh.exe", []);
        Assert.Null(result);
    }

    [Fact]
    public void FindPowerShellExecutable_ReturnsPwshWhenAvailable()
    {
        var result = PowerShellRunner.GetFullPathFromPathEnv("pwsh.exe");
        Assert.NotNull(result);
        Assert.True(result.EndsWith("pwsh.exe", StringComparison.OrdinalIgnoreCase));
    }
}

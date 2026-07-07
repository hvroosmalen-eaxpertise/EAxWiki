using EAxWiki.SchedulerUI;

namespace EAxWiki.Tests;

public class RepoLocatorTests
{
    [Fact]
    public void FindRepoRoot_FindsMarkerInParentDirectory()
    {
        var root = Path.Combine(Path.GetTempPath(), "eaxwiki_repo_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "scripts"));
            File.WriteAllText(Path.Combine(root, "scripts", "register-scheduled-task.ps1"), "");
            Directory.CreateDirectory(Path.Combine(root, "sub", "deep"));

            var startDir = Path.Combine(root, "sub", "deep");
            var result = RepoLocator.FindRepoRoot(startDir);
            Assert.Equal(root, result);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void FindRepoRoot_ReturnsNull_WhenMarkerNotFound()
    {
        var emptyDir = Path.Combine(Path.GetTempPath(), "eaxwiki_empty_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(emptyDir);
            var result = RepoLocator.FindRepoRoot(emptyDir);
            Assert.Null(result);
        }
        finally
        {
            if (Directory.Exists(emptyDir))
                Directory.Delete(emptyDir, recursive: true);
        }
    }

    [Fact]
    public void FindRepoRoot_StopsAfterTenLevels()
    {
        var baseDir = Path.Combine(Path.GetTempPath(), "eaxwiki_deep_" + Guid.NewGuid().ToString("N"));
        try
        {
            var startDir = baseDir;
            for (var i = 0; i < 15; i++)
            {
                startDir = Path.Combine(startDir, "d");
                Directory.CreateDirectory(startDir);
            }
            var result = RepoLocator.FindRepoRoot(startDir);
            Assert.Null(result);
        }
        finally
        {
            if (Directory.Exists(baseDir))
                Directory.Delete(baseDir, recursive: true);
        }
    }
}

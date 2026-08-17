using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorPathsTests : IDisposable
{
    private readonly string _dir;

    public MonitorPathsTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_mpath_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(_dir, "scripts"));
        Directory.CreateDirectory(Path.Combine(_dir, ".git"));
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void FindRepoRoot_FindsScriptsMarkerInParent()
    {
        var nested = Path.Combine(_dir, "a", "b");
        Directory.CreateDirectory(nested);
        Assert.Equal(_dir, MonitorPaths.FindRepoRoot(nested));
    }

    [Fact]
    public void StateDir_UsesInstanceHash()
    {
        var wiki = Path.Combine(_dir, "wiki");
        var stateDir = MonitorPaths.StateDir(_dir, wiki);
        Assert.Equal(Path.Combine(_dir, ".eaxwiki-monitor", EAxWiki.Core.Monitoring.InstanceHash.Compute(wiki)), stateDir);
    }
}

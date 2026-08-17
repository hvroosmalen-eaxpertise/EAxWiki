using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class InstanceHashTests
{
    [Fact]
    public void Compute_Is12HexChars()
    {
        var hash = InstanceHash.Compute(@"C:\repo\wiki");
        Assert.Equal(12, hash.Length);
        Assert.Matches("^[0-9a-f]{12}$", hash);
    }

    [Fact]
    public void Compute_IsCaseInsensitiveOnWikiDir()
    {
        Assert.Equal(InstanceHash.Compute(@"C:\repo\wiki"), InstanceHash.Compute(@"c:\REPO\WIKI"));
    }

    [Fact]
    public void Compute_DifferentPaths_Differ()
    {
        Assert.NotEqual(InstanceHash.Compute(@"C:\repo\wiki"), InstanceHash.Compute(@"C:\repo\wiki2"));
    }
}

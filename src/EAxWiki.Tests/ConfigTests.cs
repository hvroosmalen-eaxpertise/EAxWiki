using EAxWiki;

namespace EAxWiki.Tests;

public class ConfigTests
{
    [Fact]
    public void Load_NoArgs_AllDefaults()
    {
        var cfg = new Config();
        cfg.Load([]);
        Assert.Equal("", cfg.RepositoryPath);
        Assert.Equal("wiki", cfg.OutputPath);
        Assert.False(cfg.HelpRequested);
        Assert.False(cfg.Verbose);
        Assert.False(cfg.Force);
        Assert.False(cfg.ApiMode);
        Assert.Equal(0, cfg.ApiPort);
        Assert.Equal(0, cfg.WikiPort);
    }

    [Fact]
    public void Load_RepoShort_SetsRepositoryPath()
    {
        var cfg = new Config();
        cfg.Load(["-r", @"C:\model.qea"]);
        Assert.Equal(@"C:\model.qea", cfg.RepositoryPath);
    }

    [Fact]
    public void Load_RepoLong_SetsRepositoryPath()
    {
        var cfg = new Config();
        cfg.Load(["--repo", @"C:\model.qea"]);
        Assert.Equal(@"C:\model.qea", cfg.RepositoryPath);
    }

    [Fact]
    public void Load_OutputShort_SetsOutputPath()
    {
        var cfg = new Config();
        cfg.Load(["-o", "output"]);
        Assert.Equal("output", cfg.OutputPath);
    }

    [Fact]
    public void Load_OutputLong_SetsOutputPath()
    {
        var cfg = new Config();
        cfg.Load(["--output", "output"]);
        Assert.Equal("output", cfg.OutputPath);
    }

    [Fact]
    public void Load_NameShort_SetsRepositoryName()
    {
        var cfg = new Config();
        cfg.Load(["-n", "MyRepo"]);
        Assert.Equal("MyRepo", cfg.RepositoryName);
    }

    [Fact]
    public void Load_PackageShort_SetsPackageFilter()
    {
        var cfg = new Config();
        cfg.Load(["-p", "ArchiMate"]);
        Assert.Equal("ArchiMate", cfg.PackageFilter);
    }

    [Fact]
    public void Load_FlagWithMissingValue_Throws()
    {
        var cfg = new Config();
        Action act = () => cfg.Load(["--repo"]);
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Load_ForceFlags_SetsForce()
    {
        var cfg = new Config();
        cfg.Load(["-f"]);
        Assert.True(cfg.Force);
    }

    [Fact]
    public void Load_VerboseFlags_SetsVerbose()
    {
        var cfg = new Config();
        cfg.Load(["-v"]);
        Assert.True(cfg.Verbose);
    }

    [Fact]
    public void Load_JsonFlags_SetsJsonExport()
    {
        var cfg = new Config();
        cfg.Load(["-j"]);
        Assert.True(cfg.JsonExport);
    }

    [Fact]
    public void Load_WritebackFlags_SetsWriteBack()
    {
        var cfg = new Config();
        cfg.Load(["-w"]);
        Assert.True(cfg.WriteBack);
    }

    [Fact]
    public void Load_HelpFlags_SetsHelpRequested()
    {
        var cfg = new Config();
        cfg.Load(["-h"]);
        Assert.True(cfg.HelpRequested);
    }

    [Fact]
    public void Load_ApiFlag_SetsApiModeWithDefaultPort()
    {
        var cfg = new Config();
        cfg.Load(["--api"]);
        Assert.True(cfg.ApiMode);
        Assert.Equal(8001, cfg.ApiPort);
    }

    [Fact]
    public void Load_ApiPortFlag_SetsApiPort()
    {
        var cfg = new Config();
        cfg.Load(["--api-port", "9000"]);
        Assert.Equal(9000, cfg.ApiPort);
    }

    [Fact]
    public void Load_ApiPortWithoutApi_NoAutoApiMode()
    {
        var cfg = new Config();
        cfg.Load(["--api-port", "9000"]);
        Assert.Equal(9000, cfg.ApiPort);
        Assert.False(cfg.ApiMode);
    }

    [Fact]
    public void Load_WikiPortFlag_SetsWikiPort()
    {
        var cfg = new Config();
        cfg.Load(["--wiki-port", "8080"]);
        Assert.Equal(8080, cfg.WikiPort);
    }

    [Fact]
    public void Load_AllFlagsTogether_ParsesCorrectly()
    {
        var cfg = new Config();
        cfg.Load(["--repo", "r", "--output", "out", "-f", "-v", "--json",
                  "--writeback", "--api", "--api-port", "9001", "--wiki-port", "8080",
                  "--package", "pkg1", "--name", "MyRepo"]);
        Assert.Equal("r", cfg.RepositoryPath);
        Assert.Equal("out", cfg.OutputPath);
        Assert.Equal("pkg1", cfg.PackageFilter);
        Assert.Equal("MyRepo", cfg.RepositoryName);
        Assert.True(cfg.Force);
        Assert.True(cfg.Verbose);
        Assert.True(cfg.JsonExport);
        Assert.True(cfg.WriteBack);
        Assert.True(cfg.ApiMode);
        Assert.Equal(9001, cfg.ApiPort);
        Assert.Equal(8080, cfg.WikiPort);
    }

    [Fact]
    public void Load_UnknownFlag_Ignored()
    {
        var cfg = new Config();
        cfg.Load(["--unknown-flag"]);
        Assert.Equal("", cfg.RepositoryPath);
    }
}

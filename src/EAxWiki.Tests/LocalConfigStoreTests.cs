using EAxWiki.Core.Configuration;

namespace EAxWiki.Tests;

public class LocalConfigStoreTests : IDisposable
{
    private readonly string _dir;

    public LocalConfigStoreTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_config_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesAllFields()
    {
        var config = new LocalConfigStore.Config
        {
            RepoPath = @"C:\Models\repo.qea",
            WebhookUrl = "https://hooks.slack.com/ABC",
            TeamsWebhookUrl = "https://outlook.office.com/DEF",
            WikiPort = 8000,
            ApiPort = 8001
        };
        var path = Path.Combine(_dir, ".eaxwiki");
        LocalConfigStore.Save(path, config);

        var loaded = LocalConfigStore.Load(path, out var wasLegacy);
        Assert.False(wasLegacy);
        Assert.Equal(config.RepoPath, loaded.RepoPath);
        Assert.Equal(config.WebhookUrl, loaded.WebhookUrl);
        Assert.Equal(config.TeamsWebhookUrl, loaded.TeamsWebhookUrl);
        Assert.Equal(config.WikiPort, loaded.WikiPort);
        Assert.Equal(config.ApiPort, loaded.ApiPort);
    }

    [Fact]
    public void SaveAndLoad_MinimalConfig_RoundTrips()
    {
        var config = new LocalConfigStore.Config { RepoPath = @"C:\Model.qea" };
        var path = Path.Combine(_dir, ".eaxwiki");
        LocalConfigStore.Save(path, config);

        var loaded = LocalConfigStore.Load(path, out var wasLegacy);
        Assert.False(wasLegacy);
        Assert.Equal(config.RepoPath, loaded.RepoPath);
        Assert.Null(loaded.WebhookUrl);
    }

    [Fact]
    public void SaveAndLoad_EmptyRepoPath_SavesAndLoads()
    {
        var config = new LocalConfigStore.Config { RepoPath = "" };
        var path = Path.Combine(_dir, ".eaxwiki");
        LocalConfigStore.Save(path, config);

        var loaded = LocalConfigStore.Load(path, out _);
        Assert.Equal("", loaded.RepoPath);
    }

    [Fact]
    public void Load_LegacyPlaintext_DetectsWasLegacy()
    {
        var path = Path.Combine(_dir, ".eaxwiki");
        File.WriteAllText(path, @"C:\Models\repo.qea");

        var loaded = LocalConfigStore.Load(path, out var wasLegacy);
        Assert.True(wasLegacy);
        Assert.Equal(@"C:\Models\repo.qea", loaded.RepoPath);
    }

    [Fact]
    public void Load_PlainJsonWithoutEncryption_Loads()
    {
        var path = Path.Combine(_dir, ".eaxwiki");
        var json = """{"RepoPath":"C:\\repo.qea","WebhookUrl":"https://hooks.example.com/abc"}""";
        File.WriteAllText(path, json);

        var loaded = LocalConfigStore.Load(path, out var wasLegacy);
        Assert.False(wasLegacy);
        Assert.Equal(@"C:\repo.qea", loaded.RepoPath);
        Assert.Equal("https://hooks.example.com/abc", loaded.WebhookUrl);
    }

    [Fact]
    public void Load_NonExistentFile_Throws()
    {
        var path = Path.Combine(_dir, "nonexistent.eaxwiki");
        Assert.Throws<FileNotFoundException>(() => LocalConfigStore.Load(path, out _));
    }

    [Fact]
    public void Load_InvalidBase64_FallsBackToPlaintext()
    {
        var path = Path.Combine(_dir, ".eaxwiki");
        File.WriteAllText(path, "!!!not-base64!!!");

        var loaded = LocalConfigStore.Load(path, out var wasLegacy);
        Assert.True(wasLegacy);
        Assert.Equal("!!!not-base64!!!", loaded.RepoPath);
    }
}

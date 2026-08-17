using EAxWiki.Core.Configuration;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorOptionsResolverTests
{
    private const string RepoRoot = @"C:\repos\EAxWiki";
    private static LocalConfigStore.Config File(
        int? wikiPort = null, int? apiPort = null, int? llmPort = null, string? aiMode = null,
        string? aiEndpoint = null, string? repoPath = null, string? brand = null,
        string? llamaExePath = null, string? llamaModelPath = null)
    {
        var c = new LocalConfigStore.Config
        {
            WikiPort = wikiPort, ApiPort = apiPort, LlmPort = llmPort,
            AiMode = aiMode, AiEndpoint = aiEndpoint, RepoPath = repoPath,
            Brand = brand, LlamaExePath = llamaExePath, LlamaModelPath = llamaModelPath,
        };
        return c;
    }

    private static MonitorOptions Resolve(CliOptions cli, LocalConfigStore.Config? file = null,
        Func<string, string?>? getEnv = null) =>
        MonitorOptionsResolver.Resolve(cli, RepoRoot, getEnv ?? (_ => null), file);

    [Fact]
    public void NoCliNoFile_AllDefaults()
    {
        var o = Resolve(new CliOptions());
        Assert.Equal(8000, o.WikiPort);
        Assert.Equal(0, o.ApiPort);
        Assert.Equal(8080, o.LlmPort);
        Assert.Equal(3, o.MaxRetries);
        Assert.Equal(30, o.RetryDelaySeconds);
        Assert.Equal(0.5, o.MinElementFraction);
        Assert.Equal(30, o.ExportIntervalMinutes);
        Assert.Equal(30, o.CheckIntervalSeconds);
        Assert.True(o.NotifyOnStart);
        Assert.False(o.Force);
        Assert.Equal(0, o.ForceEveryNRuns);
        Assert.Equal(Path.Combine(RepoRoot, "wiki"), o.WikiDir);
        Assert.Equal("none", o.AiMode);
        Assert.Null(o.WebhookUrl);
        Assert.Null(o.RepoPath);
    }

    [Fact]
    public void File_WikiPort_Applies()
    {
        var o = Resolve(new CliOptions(), File(wikiPort: 8080));
        Assert.Equal(8080, o.WikiPort);
    }

    [Fact]
    public void PortQuirk_Cli8000AndFilePortDiffers_UsesFilePort()
    {
        var o = Resolve(new CliOptions { Port = 8000 }, File(wikiPort: 9090));
        Assert.Equal(9090, o.WikiPort);
    }

    [Fact]
    public void PortQuirk_CliExplicit_OverridesFile()
    {
        var o = Resolve(new CliOptions { Port = 7777 }, File(wikiPort: 9090));
        Assert.Equal(7777, o.WikiPort);
    }

    [Fact]
    public void ApiPort_ComesFromFileOnly()
    {
        Assert.Equal(8001, Resolve(new CliOptions(), File(apiPort: 8001)).ApiPort);
        Assert.Equal(0, Resolve(new CliOptions(), File()).ApiPort);
    }

    [Fact]
    public void LlmPort_CliOverridesFileAndDefault()
    {
        Assert.Equal(9090, Resolve(new CliOptions { LlmPort = 9090 }, File(llmPort: 8080)).LlmPort);
        Assert.Equal(8181, Resolve(new CliOptions(), File(llmPort: 8181)).LlmPort);
        Assert.Equal(8080, Resolve(new CliOptions(), File()).LlmPort);
    }

    [Fact]
    public void Webhook_EnvBeatsFile_ButCliBeatsEnv()
    {
        var o = Resolve(new CliOptions(),
            File(brand: "file-brand"),
            _ => "env-brand");
        Assert.Equal("env-brand", o.Brand);

        var o2 = Resolve(new CliOptions { Brand = "cli-brand" },
            File(brand: "file-brand"),
            _ => "env-brand");
        Assert.Equal("cli-brand", o2.Brand);
    }

    [Fact]
    public void RepoPath_CliBeatsFile()
    {
        Assert.Equal(@"C:\models\repo.qea",
            Resolve(new CliOptions { Repo = @"C:\models\repo.qea" }, File(repoPath: @"C:\old\repo.qea")).RepoPath);
        Assert.Equal(@"C:\old\repo.qea",
            Resolve(new CliOptions(), File(repoPath: @"C:\old\repo.qea")).RepoPath);
    }

    [Fact]
    public void AiMode_InferredLocal_FromLocalhostEndpointAndExistingExe()
    {
        // Write a real temp file so File.Exists(LlamaExePath) is true.
        var dir = Path.Combine(Path.GetTempPath(), "eaxwiki_ai_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var exe = Path.Combine(dir, "llama-server.exe");
            System.IO.File.WriteAllText(exe, "x");
            var o = Resolve(new CliOptions(), File(
                aiEndpoint: "http://localhost:8080/v1",
                aiMode: "none",
                llamaExePath: exe,
                llamaModelPath: Path.Combine(dir, "model.gguf")));
            Assert.Equal("local", o.AiMode);
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public void AiMode_NotInferred_WhenExeMissing()
    {
        var o = Resolve(new CliOptions(), File(
            aiEndpoint: "http://localhost:8080/v1",
            aiMode: "none",
            llamaExePath: @"E:\missing\llama-server.exe",
            llamaModelPath: @"E:\missing\model.gguf"));
        Assert.Equal("none", o.AiMode);
    }

    [Fact]
    public void OutputDir_Absolute_StaysAbsolute()
    {
        var o = Resolve(new CliOptions { OutputDir = @"D:\out\wiki" }, null);
        Assert.Equal(@"D:\out\wiki", o.WikiDir);
    }

    [Fact]
    public void OutputDir_Relative_JoinsRepoRoot()
    {
        var o = Resolve(new CliOptions { OutputDir = "mywiki" }, null);
        Assert.Equal(Path.Combine(RepoRoot, "mywiki"), o.WikiDir);
    }

    [Fact]
    public void LlamaDefaults_ApplyWhenMissing()
    {
        var o = Resolve(new CliOptions(), File(aiMode: "local"));
        Assert.Equal(@"E:\llama-cpp\llama-server.exe", o.LlamaExePath);
        Assert.Equal(@"E:\models\llama-3.2-3b-q4.gguf", o.LlamaModelPath);
    }
}
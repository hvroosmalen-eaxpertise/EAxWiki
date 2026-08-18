using System.CommandLine;
using System.CommandLine.Help;
using EAxWiki;

namespace EAxWiki.Tests;

public class CommandLineTests
{
    private static Config Parse(params string[] args) =>
        CommandLine.ToConfig(CommandLine.BuildCommand().Parse(args));

    private static ParseResult ParseResult(params string[] args) =>
        CommandLine.BuildCommand().Parse(args);

    [Fact]
    public void NoArgs_AllDefaults()
    {
        var cfg = Parse();
        Assert.Equal("", cfg.RepositoryPath);
        Assert.Equal("wiki", cfg.OutputPath);
        Assert.Null(cfg.RepositoryName);
        Assert.Null(cfg.PackageFilter);
        Assert.False(cfg.Verbose);
        Assert.False(cfg.Force);
        Assert.False(cfg.JsonExport);
        Assert.False(cfg.WriteBack);
        Assert.False(cfg.ApiMode);
        Assert.Equal(0, cfg.ApiPort);
        Assert.Equal(0, cfg.WikiPort);
        Assert.Equal(60, cfg.ApiRateLimitPerMinute);
        Assert.Equal("", cfg.Brand);
        Assert.Equal("", cfg.AiEndpoint);
        Assert.Equal("llama-3.2-3b", cfg.AiModel);
        Assert.Equal("", cfg.AiKey);
        Assert.Null(cfg.CertPath);
        Assert.Null(cfg.CertPassword);
    }

    [Theory]
    [InlineData("-r")]
    [InlineData("--repo")]
    public void RepoFlag_SetsRepositoryPath(string flag)
    {
        Assert.Equal(@"C:\model.qea", Parse(flag, @"C:\model.qea").RepositoryPath);
    }

    [Fact]
    public void BarePositionalRepo_SetsRepositoryPath()
    {
        Assert.Equal("model.qea", Parse("model.qea").RepositoryPath);
    }

    [Fact]
    public void BarePositionalConnectionString_SetsRepositoryPath()
    {
        Assert.Equal("DBType=postgresql;Database=foo", Parse("DBType=postgresql;Database=foo").RepositoryPath);
    }

    [Fact]
    public void RepoFlag_WinsOverBarePositional()
    {
        Assert.Equal("a.qea", Parse("b.qea", "--repo", "a.qea").RepositoryPath);
    }

    [Fact]
    public void EmptyRepoValue_StaysEmpty()
    {
        Assert.Equal("", Parse("--repo", "").RepositoryPath);
    }

    [Theory]
    [InlineData("-o")]
    [InlineData("--output")]
    public void OutputFlag_SetsOutputPath(string flag)
    {
        Assert.Equal("output", Parse(flag, "output").OutputPath);
    }

    [Theory]
    [InlineData("-n")]
    [InlineData("--name")]
    public void NameFlag_SetsRepositoryName(string flag)
    {
        Assert.Equal("MyRepo", Parse(flag, "MyRepo").RepositoryName);
    }

    [Theory]
    [InlineData("-p")]
    [InlineData("--package")]
    public void PackageFlag_SetsPackageFilter(string flag)
    {
        Assert.Equal("ArchiMate", Parse(flag, "ArchiMate").PackageFilter);
    }

    [Theory]
    [InlineData("-f")]
    [InlineData("--force")]
    public void ForceFlags_SetForce(string flag) => Assert.True(Parse(flag).Force);

    [Theory]
    [InlineData("-v")]
    [InlineData("--verbose")]
    public void VerboseFlags_SetVerbose(string flag) => Assert.True(Parse(flag).Verbose);

    [Theory]
    [InlineData("-j")]
    [InlineData("--json")]
    public void JsonFlags_SetJsonExport(string flag) => Assert.True(Parse(flag).JsonExport);

    [Theory]
    [InlineData("-w")]
    [InlineData("--writeback")]
    public void WritebackFlags_SetWriteBack(string flag) => Assert.True(Parse(flag).WriteBack);

    [Fact]
    public void MissingValue_IsParseError()
    {
        Assert.NotEmpty(ParseResult("--repo").Errors);
    }

    [Fact]
    public void UnknownFlag_IsParseError()
    {
        Assert.NotEmpty(ParseResult("--unknown-flag").Errors);
    }

    [Theory]
    [InlineData("--froce")]
    [InlineData("-x")]
    public void TypoFlag_IsParseError(string flag)
    {
        Assert.NotEmpty(ParseResult(flag).Errors);
    }

    [Fact]
    public void ApiFlag_SetsApiModeWithDefaultPort()
    {
        var cfg = Parse("--api");
        Assert.True(cfg.ApiMode);
        Assert.Equal(8001, cfg.ApiPort);
    }

    [Fact]
    public void ApiPortWithoutApi_NoAutoApiMode()
    {
        var cfg = Parse("--api-port", "9000");
        Assert.Equal(9000, cfg.ApiPort);
        Assert.False(cfg.ApiMode);
    }

    [Fact]
    public void ApiWithApiPort_SetsBoth()
    {
        var cfg = Parse("--api", "--api-port", "9000");
        Assert.True(cfg.ApiMode);
        Assert.Equal(9000, cfg.ApiPort);
    }

    [Fact]
    public void WikiPortFlag_SetsWikiPort()
    {
        Assert.Equal(8080, Parse("--wiki-port", "8080").WikiPort);
    }

    [Theory]
    [InlineData("--api-port", "0")]
    [InlineData("--api-port", "65536")]
    [InlineData("--api-port", "abc")]
    [InlineData("--wiki-port", "0")]
    [InlineData("--wiki-port", "99999")]
    public void InvalidPorts_AreParseErrors(string flag, string value)
    {
        Assert.NotEmpty(ParseResult(flag, value).Errors);
    }

    [Fact]
    public void BrandFlag_SetsBrand()
    {
        Assert.Equal("eursura", Parse("--brand", "eursura").Brand);
    }

    [Fact]
    public void CertAndAiFlags_SetValues()
    {
        var cfg = Parse(
            "--cert", "cert.pfx", "--cert-password", "secret",
            "--ai-endpoint", "http://localhost:11434/v1", "--ai-model", "gpt-x", "--ai-key", "k");
        Assert.Equal("cert.pfx", cfg.CertPath);
        Assert.Equal("secret", cfg.CertPassword);
        Assert.Equal("http://localhost:11434/v1", cfg.AiEndpoint);
        Assert.Equal("gpt-x", cfg.AiModel);
        Assert.Equal("k", cfg.AiKey);
    }

    [Fact]
    public void AllFlagsTogether_ParsesCorrectly()
    {
        var cfg = Parse("--repo", "r", "--output", "out", "-f", "-v", "--json",
            "--writeback", "--api", "--api-port", "9001", "--wiki-port", "8080",
            "--package", "pkg1", "--name", "MyRepo", "--brand", "eursura");
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
        Assert.Equal("eursura", cfg.Brand);
    }

    [Fact]
    public void DuplicateOutput_IsParseError()
    {
        // SCL 2.0.11 rejects repeated single-valued options (old Config.Load silently took the last).
        Assert.NotEmpty(ParseResult("--output", "wiki1", "--output", "wiki2").Errors);
    }

    [Fact]
    public void DuplicateApiPort_IsParseError()
    {
        Assert.NotEmpty(ParseResult("--api-port", "8000", "--api-port", "9000").Errors);
    }

    [Fact]
    public void DuplicateBareRepo_IsParseError()
    {
        Assert.NotEmpty(ParseResult("a.qea", "b.qea").Errors);
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    [InlineData("-?")]
    [InlineData("/?")]
    public void HelpFlags_AreHelpActions(string flag)
    {
        var r = ParseResult(flag);
        Assert.Empty(r.Errors);
        Assert.IsType<HelpAction>(r.Action);
    }
}

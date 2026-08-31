using EAxWiki.Export;
using EAxWiki.Export.Exporters;

namespace EAxWiki.Tests;

public class StatusPagesNavTests : IDisposable
{
    private readonly string _outPath;

    public StatusPagesNavTests()
    {
        _outPath = Path.Combine(Path.GetTempPath(), "eaxwiki_statusnav_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_outPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_outPath))
            Directory.Delete(_outPath, recursive: true);
    }

    private async Task WritePages()
    {
        await new InfrastructureWriter(new FileOutputWriter()).WritePagesFileAsync(_outPath);
    }

    [Fact]
    public async Task NoStatusFiles_OmitsErrorConfigAndHealthEntries()
    {
        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.DoesNotContain("Error Log", pages);
        Assert.DoesNotContain("Configuration", pages);
        Assert.DoesNotContain("Pipeline Health", pages);
    }

    [Fact]
    public async Task WithErrorAndConfigFiles_IncludesEntries()
    {
        var statusDir = Path.Combine(_outPath, "status");
        Directory.CreateDirectory(statusDir);
        File.WriteAllText(Path.Combine(statusDir, "errors.md"), "x");
        File.WriteAllText(Path.Combine(statusDir, "config.md"), "x");

        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.Contains("  - Error Log: status/errors.html", pages);
        Assert.Contains("  - Configuration: status/config.html", pages);
    }

    [Fact]
    public async Task WithHealthFile_IncludesPipelineHealthEntry()
    {
        var statusDir = Path.Combine(_outPath, "status");
        Directory.CreateDirectory(statusDir);
        File.WriteAllText(Path.Combine(statusDir, "health.md"), "x");

        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.Contains("  - Pipeline Health: status/health.html", pages);
    }

    [Fact]
    public async Task NoRootPackages_KeepsBareRepositoryLink()
    {
        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.Contains("  - Repository: ''", pages);
    }

    [Fact]
    public async Task WithRootPackages_EmitsRepositorySectionWithNestedPackages()
    {
        await new InfrastructureWriter(new FileOutputWriter())
            .WritePagesFileAsync(_outPath, new[] { "PackageA", "PackageB" });

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.DoesNotContain("  - Repository: ''", pages);
        Assert.Contains("  - Repository:", pages);
        Assert.Contains("    - PackageA", pages);
        Assert.Contains("    - PackageB", pages);
        // No trailing slash — awesome-pages rejects "Name/" in nested nav entries with
        // NavEntryNotFound. Bare name resolves to the subdirectory and inherits its .pages title.
        Assert.DoesNotContain("    - PackageA/", pages);
        // Sibling nav entries survive the change.
        Assert.Contains("  - Diagrams: diagrams/", pages);
    }
}
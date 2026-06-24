using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class ExportIntegrationTests
{
    private sealed class InMemoryWriter : IOutputWriter
    {
        public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
        public readonly HashSet<string> Directories = new(StringComparer.OrdinalIgnoreCase);

        public Task CreateDirectoryAsync(string path) { Directories.Add(path); return Task.CompletedTask; }
        public Task WriteFileAsync(string filePath, string content) { Files[Normalize(filePath)] = content; return Task.CompletedTask; }

        private static string Normalize(string path) => path.Replace('\\', '/');
    }

    private static EaRepository MinimalRepository(string pkgName = "MyPackage", string elemName = "MyElement")
    {
        var element = new EaElement { Id = 1, Name = elemName, Type = "Class", Stereotype = "ESRS::Disclosure" };
        var package = new EaPackage { Id = 10, Name = pkgName, Elements = { element } };
        return new EaRepository { RootPackages = { package } };
    }

    private static string OutputPath { get; } = Path.Combine(Path.GetTempPath(), "eaxwiki_test_" + Guid.NewGuid().ToString("N"));

    private static string Normalize(string path) => path.Replace('\\', '/');

    [Fact]
    public async Task Export_CreatesRootIndexFile()
    {
        var writer = new InMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository();
        var outPath = OutputPath;

        await exporter.ExportAsync(repo, null, outPath);

        var expected = Normalize(Path.Combine(outPath, "index.md"));
        Assert.True(writer.Files.ContainsKey(expected), $"root index.md should be created. Keys: {string.Join(", ", writer.Files.Keys)}");
    }

    [Fact]
    public async Task Export_CreatesPackageIndexFile()
    {
        var writer = new InMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository("MyPackage");
        var outPath = OutputPath;

        await exporter.ExportAsync(repo, null, outPath);

        var expected = Normalize(Path.Combine(outPath, "MyPackage", "index.md"));
        Assert.True(writer.Files.ContainsKey(expected), $"package index.md should be created. Keys: {string.Join(", ", writer.Files.Keys)}");
    }

    [Fact]
    public async Task Export_CreatesElementPageFile()
    {
        var writer = new InMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository("MyPackage", "MyElement");
        var outPath = OutputPath;

        await exporter.ExportAsync(repo, null, outPath);

        var expected = Normalize(Path.Combine(outPath, "MyPackage", "MyElement.md"));
        Assert.True(writer.Files.ContainsKey(expected), $"element page should be created. Keys: {string.Join(", ", writer.Files.Keys)}");
    }

    [Fact]
    public async Task Export_ElementPageContainsElementName()
    {
        var writer = new InMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository("Pkg", "SomeElement");
        var outPath = OutputPath;

        await exporter.ExportAsync(repo, null, outPath);

        var key = Normalize(Path.Combine(outPath, "Pkg", "SomeElement.md"));
        var content = writer.Files[key];
        Assert.Contains("SomeElement", content);
    }

    [Fact]
    public async Task Export_PackageIndexListsElement()
    {
        var writer = new InMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository("Pkg", "Alpha");
        var outPath = OutputPath;

        await exporter.ExportAsync(repo, null, outPath);

        var key = Normalize(Path.Combine(outPath, "Pkg", "index.md"));
        var content = writer.Files[key];
        Assert.Contains("Alpha", content);
    }

    [Fact]
    public async Task Export_NamesWithSpecialChars_SanitizedInPath()
    {
        var writer = new InMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository("My<Package>", "My|Element");

        await exporter.ExportAsync(repo, null, OutputPath);

        Assert.DoesNotContain(writer.Files.Keys, k => k.Contains('<') || k.Contains('>') || k.Contains('|'));
    }
}

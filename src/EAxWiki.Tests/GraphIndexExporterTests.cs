using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export;
using EAxWiki.Export.Exporters;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace EAxWiki.Tests;

public class GraphIndexExporterTests
{
    private sealed class MemoryWriter : IOutputWriter
    {
        public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
        public Task CreateDirectoryAsync(string path, CancellationToken ct = default) => Task.CompletedTask;
        public Task WriteFileAsync(string filePath, string content, CancellationToken ct = default)
        {
            Files[filePath.Replace('\\', '/')] = content;
            return Task.CompletedTask;
        }
    }

    private static ExportContext MakeContext(List<(EaElement, string)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var lookup = new Dictionary<int, (EaElement, string)>();
        foreach (var (el, dir) in elements)
            lookup[el.Id] = (el, dir);
        var pkgLookup = packageLookup;
        return new ExportContext(
            OutputPath: "C:\\out",
            Elements: elements,
            ElementLookup: lookup,
            AllDiagrams: [],
            DiagramIndex: [],
            IncomingIndex: [],
            PackageLookup: pkgLookup
        );
    }

    private static EaElement MakeElement(int id, string name, int pkgId, string type = "Class", string stereotype = "ArchiMate_BusinessActor")
    {
        return new EaElement
        {
            Id = id,
            Name = name,
            Type = type,
            Stereotype = stereotype,
            PackageId = pkgId,
        };
    }

    [Fact]
    public async Task Produces_Valid_Json_With_Nodes_And_Edges()
    {
        var pkg = ("Pkg", (int?)null);
        var a = MakeElement(1, "Alpha", 10, stereotype: "ArchiMate_BusinessActor");
        var b = MakeElement(2, "Beta", 10, stereotype: "ArchiMate_ApplicationComponent");
        a.Connectors.Add(new EaConnector { Id = 101, SourceId = 1, TargetId = 2, Name = "serves", Type = "Dependency" });
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg"), (b, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        var json = writer.Files["C:/out/graph-index.json"];
        Assert.NotNull(json);

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var nodes = root.GetProperty("nodes").EnumerateArray().ToList();
        Assert.Contains(nodes, n => n.GetProperty("id").GetInt32() == 1);
        Assert.Contains(nodes, n => n.GetProperty("id").GetInt32() == 2);

        var edges = root.GetProperty("edges").EnumerateArray().ToList();
        Assert.Single(edges);
        Assert.Equal(101, edges[0].GetProperty("id").GetInt32());
        Assert.Equal(1, edges[0].GetProperty("source").GetInt32());
        Assert.Equal(2, edges[0].GetProperty("target").GetInt32());
    }

    [Fact]
    public async Task Edges_Deduplicated_By_ConnectorId()
    {
        var pkg = ("Pkg", (int?)null);
        var a = MakeElement(1, "Alpha", 10);
        var b = MakeElement(2, "Beta", 10);
        // Both endpoints reference the same connector.
        a.Connectors.Add(new EaConnector { Id = 101, SourceId = 1, TargetId = 2, Name = "link", Type = "Association" });
        b.Connectors.Add(new EaConnector { Id = 101, SourceId = 1, TargetId = 2, Name = "link", Type = "Association" });
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg"), (b, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var edges = doc.RootElement.GetProperty("edges").EnumerateArray().ToList();
        Assert.Single(edges);
    }

    [Fact]
    public async Task Node_Has_Layer_From_Stereotype()
    {
        var a = MakeElement(1, "Alpha", 10, stereotype: "ArchiMate_BusinessActor");
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var node = doc.RootElement.GetProperty("nodes")[0];
        Assert.Equal("business", node.GetProperty("layer").GetString());
    }

    [Fact]
    public async Task Node_Url_Is_RootRelative_Html()
    {
        var a = MakeElement(1, "My Element", 10);
        var ctx = MakeContext(
            [(a, "C:\\out\\MyPkg")],
            new() { [10] = ("MyPkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var node = doc.RootElement.GetProperty("nodes")[0];
        Assert.Equal("MyPkg/My Element.html", node.GetProperty("url").GetString());
    }

    [Fact]
    public async Task Package_With_Two_Elements_No_Edges_Still_Produces_Nodes()
    {
        var a = MakeElement(1, "Alpha", 10);
        var b = MakeElement(2, "Beta", 10);
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg"), (b, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        Assert.Equal(2, doc.RootElement.GetProperty("nodes").GetArrayLength());
        Assert.Equal(0, doc.RootElement.GetProperty("edges").GetArrayLength());
    }
}

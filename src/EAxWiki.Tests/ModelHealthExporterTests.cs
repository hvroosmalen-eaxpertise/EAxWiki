using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export;
using EAxWiki.Export.Exporters;

namespace EAxWiki.Tests;

public class ModelHealthExporterTests
{
    private sealed class InMemoryWriter : IOutputWriter
    {
        public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);

        public Task CreateDirectoryAsync(string path, CancellationToken ct = default) => Task.CompletedTask;
        public Task WriteFileAsync(string filePath, string content, CancellationToken ct = default)
        {
            Files[filePath.Replace('\\', '/')] = content;
            return Task.CompletedTask;
        }
    }

    private const string OutputPath = @"C:\wiki";

    private static ExportContext BuildContext(
        List<(EaElement Element, string PackageDir)> elements,
        Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>>? diagramIndex = null)
    {
        var elementLookup = LookupBuilder.BuildElementLookup(elements);
        var incomingIndex = ConnectorIndexBuilder.Build(elements);
        return new ExportContext(
            OutputPath: OutputPath,
            Elements: elements,
            ElementLookup: elementLookup,
            AllDiagrams: [],
            DiagramIndex: diagramIndex ?? new Dictionary<int, List<(EaDiagram, string)>>(),
            IncomingIndex: incomingIndex,
            PackageLookup: new Dictionary<int, (string Name, int? ParentId)>());
    }

    private static async Task<string> RunAsync(ExportContext ctx)
    {
        var writer = new InMemoryWriter();
        await new ModelHealthExporter(writer).ExportAsync(ctx);
        return writer.Files[$"{OutputPath}/status/model-health.md".Replace('\\', '/')];
    }

    [Fact]
    public async Task NoIssues_ReportsCleanState()
    {
        var elem = new EaElement { Id = 1, Name = "Healthy", PackageId = 1, Notes = "A real description.", Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }] };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("No issues found.", content);
    }

    [Fact]
    public async Task Orphan_ZeroConnectorsAndNoDiagram_IsFlagged()
    {
        var elem = new EaElement { Id = 1, Name = "Lonely", PackageId = 1, Connectors = [] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")]);

        var content = await RunAsync(ctx);

        Assert.Contains("## Orphan Elements (1)", content);
        Assert.Contains("Lonely", content);
    }

    [Fact]
    public async Task Orphan_NoConnectorsButOnDiagram_IsNotFlagged()
    {
        var elem = new EaElement { Id = 1, Name = "OnDiagramOnly", PackageId = 1, Connectors = [] };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Orphan Elements (0)", content);
    }

    [Fact]
    public async Task Orphan_HasConnectorButNoDiagram_IsNotFlagged()
    {
        var elem = new EaElement { Id = 1, Name = "ConnectedOnly", PackageId = 1, Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")]);

        var content = await RunAsync(ctx);

        Assert.Contains("## Orphan Elements (0)", content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("<p></p>")]
    public async Task MissingDescription_EmptyOrWhitespaceOnlyNotes_IsFlagged(string? notes)
    {
        var elem = new EaElement { Id = 1, Name = "NoNotes", PackageId = 1, Notes = notes, Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }] };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Missing Descriptions (1)", content);
    }

    [Fact]
    public async Task MissingDescription_NonEmptyNotes_IsNotFlagged()
    {
        var elem = new EaElement { Id = 1, Name = "HasNotes", PackageId = 1, Notes = "<p>Real content.</p>", Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }] };
        // A genuinely orphaned sibling keeps the report from taking the all-clean shortcut, so this
        // test actually exercises the Missing Descriptions section header rather than skipping past it.
        var orphan = new EaElement { Id = 2, Name = "Lonely", PackageId = 1, Notes = "<p>Has notes, just no relationships.</p>", Connectors = [] };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg"), (orphan, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Missing Descriptions (0)", content);
    }

    [Fact]
    public async Task Stale_StatusSetAndModifiedOverThreshold_IsFlagged()
    {
        var elem = new EaElement
        {
            Id = 1, Name = "OldProposed", PackageId = 1, Status = "Proposed",
            ModifiedDate = DateTime.Now.AddDays(-100),
            Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }]
        };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Untouched 90+ Days (1)", content);
        Assert.Contains("Proposed", content);
    }

    [Fact]
    public async Task Stale_StatusSetButRecentlyModified_IsNotFlagged()
    {
        var elem = new EaElement
        {
            Id = 1, Name = "RecentProposed", PackageId = 1, Status = "Proposed",
            ModifiedDate = DateTime.Now.AddDays(-1),
            Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }]
        };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Untouched 90+ Days (0)", content);
    }

    [Fact]
    public async Task Stale_NoStatusSet_IsNotFlaggedEvenIfOld()
    {
        var elem = new EaElement
        {
            Id = 1, Name = "NoStatusOld", PackageId = 1, Status = "",
            ModifiedDate = DateTime.Now.AddDays(-500),
            Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 2 }]
        };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(new EaDiagram { Id = 1 }, "pkg")] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Untouched 90+ Days (0)", content);
    }

    [Fact]
    public async Task Duplicate_SameNameSamePackage_IsFlagged()
    {
        var e1 = new EaElement { Id = 1, Name = "Dup", PackageId = 1, Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 9 }] };
        var e2 = new EaElement { Id = 2, Name = "Dup", PackageId = 1, Connectors = [new EaConnector { Id = 6, SourceId = 2, TargetId = 9 }] };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>>
        {
            [1] = [(new EaDiagram { Id = 1 }, "pkg")],
            [2] = [(new EaDiagram { Id = 1 }, "pkg")],
        };
        var ctx = BuildContext([(e1, @"C:\wiki\Pkg"), (e2, @"C:\wiki\Pkg")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Duplicate Names Within a Package (1 name, 2 elements)", content);
    }

    [Fact]
    public async Task Duplicate_SameNameDifferentPackage_IsNotFlagged()
    {
        var e1 = new EaElement { Id = 1, Name = "SameName", PackageId = 1, Connectors = [new EaConnector { Id = 5, SourceId = 1, TargetId = 9 }] };
        var e2 = new EaElement { Id = 2, Name = "SameName", PackageId = 2, Connectors = [new EaConnector { Id = 6, SourceId = 2, TargetId = 9 }] };
        var diagramIndex = new Dictionary<int, List<(EaDiagram, string)>>
        {
            [1] = [(new EaDiagram { Id = 1 }, "pkg")],
            [2] = [(new EaDiagram { Id = 1 }, "pkg")],
        };
        var ctx = BuildContext([(e1, @"C:\wiki\PkgA"), (e2, @"C:\wiki\PkgB")], diagramIndex);

        var content = await RunAsync(ctx);

        Assert.Contains("## Duplicate Names Within a Package (0 names, 0 elements)", content);
    }

    [Fact]
    public async Task EveryEntry_LinksToElementPage()
    {
        var elem = new EaElement { Id = 1, Name = "Flagged", PackageId = 1, Connectors = [] };
        var ctx = BuildContext([(elem, @"C:\wiki\Pkg")]);

        var content = await RunAsync(ctx);

        Assert.Contains("[Flagged](", content);
        Assert.Contains("Flagged.html", content);
    }
}

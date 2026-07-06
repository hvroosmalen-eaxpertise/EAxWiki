using EAxWiki.Core.Models;
using EAxWiki.Export;

namespace EAxWiki.Tests;

public class ContextBuilderTests
{
    [Fact]
    public void ElementCollector_CollectsElementsFromSinglePackage()
    {
        var pkg = new EaPackage { Name = "Root", Elements = [new EaElement { Id = 1 }, new EaElement { Id = 2 }] };

        var result = ElementCollector.Collect([pkg], @"C:\wiki");

        Assert.Equal(2, result.Count);
        Assert.All(result, e => Assert.Equal(@"C:\wiki\Root", e.PackageDir));
    }

    [Fact]
    public void ElementCollector_CollectsElementsFromNestedPackages()
    {
        var pkg = new EaPackage
        {
            Name = "Root",
            Elements = [new EaElement { Id = 1 }],
            Children =
            [
                new EaPackage
                {
                    Name = "Child",
                    Elements = [new EaElement { Id = 2 }, new EaElement { Id = 3 }]
                }
            ]
        };

        var result = ElementCollector.Collect([pkg], @"C:\wiki");

        Assert.Equal(3, result.Count);
        Assert.Equal(@"C:\wiki\Root", result[0].PackageDir);
        Assert.Equal(@"C:\wiki\Child", result[1].PackageDir);
        Assert.Equal(@"C:\wiki\Child", result[2].PackageDir);
    }

    [Fact]
    public void ElementCollector_ReturnsEmptyForPackageWithNoElements()
    {
        var pkg = new EaPackage { Name = "Empty", Elements = [] };

        var result = ElementCollector.Collect([pkg], @"C:\wiki");

        Assert.Empty(result);
    }

    [Fact]
    public void DiagramIndexBuilder_CollectsDiagramsAndBuildsIndex()
    {
        var dob1 = new EaDiagramObject { ElementId = 10 };
        var dob2 = new EaDiagramObject { ElementId = 20 };
        var pkg = new EaPackage
        {
            Name = "Root",
            Diagrams =
            [
                new EaDiagram { Id = 1, Name = "D1", DiagramObjects = [dob1, dob2] },
                new EaDiagram { Id = 2, Name = "D2", DiagramObjects = [dob1] }
            ]
        };

        var (allDiagrams, diagramIndex) = DiagramIndexBuilder.Build([pkg], @"C:\wiki");

        Assert.Equal(2, allDiagrams.Count);
        Assert.Equal(2, diagramIndex[10].Count);
        Assert.Single(diagramIndex[20]);
        Assert.Equal(1, diagramIndex[10][0].Diagram.Id);
        Assert.Equal(2, diagramIndex[10][1].Diagram.Id);
    }

    [Fact]
    public void ConnectorIndexBuilder_DeduplicatesConnectors()
    {
        // Two elements share the same connector (EA COM returns it on both sides).
        var conn = new EaConnector { Id = 99, SourceId = 1, TargetId = 2 };
        var elements = new List<(EaElement Element, string PackageDir)>
        {
            (new EaElement { Id = 1, Connectors = [conn] }, "pkg"),
            (new EaElement { Id = 2, Connectors = [conn] }, "pkg"),
        };

        var index = ConnectorIndexBuilder.Build(elements);

        // Connector appears only once, indexed under the target (2).
        Assert.Single(index);
        Assert.True(index.ContainsKey(2));
        Assert.Equal(99, index[2][0].Connector.Id);
    }

    [Fact]
    public void ConnectorIndexBuilder_ReturnsEmptyForNoConnectors()
    {
        var elements = new List<(EaElement Element, string PackageDir)>
        {
            (new EaElement { Id = 1, Connectors = [] }, "pkg"),
        };

        var index = ConnectorIndexBuilder.Build(elements);

        Assert.Empty(index);
    }

    [Fact]
    public void LookupBuilder_BuildElementLookupFindsById()
    {
        var elements = new List<(EaElement Element, string PackageDir)>
        {
            (new EaElement { Id = 10, Name = "Alpha" }, "dir/a"),
            (new EaElement { Id = 20, Name = "Beta" }, "dir/b"),
        };

        var lookup = LookupBuilder.BuildElementLookup(elements);

        Assert.Equal("Alpha", lookup[10].Element.Name);
        Assert.Equal("Beta", lookup[20].Element.Name);
        Assert.Equal("dir/b", lookup[20].PackageDir);
    }

    [Fact]
    public void LookupBuilder_BuildPackageLookupFindsById()
    {
        var packages = new List<EaPackage>
        {
            new() { Id = 1, Name = "Root", ParentId = null, Children = [new() { Id = 2, Name = "Child", ParentId = 1 }] }
        };

        var lookup = LookupBuilder.BuildPackageLookup(packages);

        Assert.Equal(("Root", null), lookup[1]);
        Assert.Equal(("Child", 1), lookup[2]);
    }

    [Fact]
    public void PackageDirCollector_IncludesDiagramsSubdir()
    {
        var pkg = new EaPackage
        {
            Name = "Root",
            Children = [new EaPackage { Name = "Child" }]
        };

        var dirs = PackageDirCollector.Collect([pkg], @"C:\wiki");

        Assert.Contains(@"C:\wiki\Root", dirs);
        Assert.Contains(@"C:\wiki\Root\diagrams", dirs);
        Assert.Contains(@"C:\wiki\Child", dirs);
        Assert.Contains(@"C:\wiki\Child\diagrams", dirs);
    }
}
